using System.Collections.Concurrent;
using DNX.Extensions.Threading;
using Shouldly;
using Xunit;

#pragma warning disable IDE0290 // No need for Primary Constructor here

namespace DNX.Extensions.Tests.Threading;

public class ProducerConsumerQueueTests
{
    private sealed class TestQueue : ProducerConsumerQueue<DisposableItem>
    {
        private readonly Action<DisposableItem> _process;
        private readonly Action<DisposableItem> _complete;
        private readonly Func<DisposableItem, bool> _canHandle;
        private readonly Action<DisposableItem> _added;
        private readonly Action<DisposableItem> _handle;

        public TestQueue(
            Action<DisposableItem> process,
            Action<DisposableItem> complete = null,
            Func<DisposableItem, bool> canHandle = null,
            bool autoCleanup = false,
            TimeSpan? timeout = null,
            Action<DisposableItem> added = null,
            Action<DisposableItem> handle = null)
            : base(autoCleanup, timeout ?? TimeSpan.MaxValue, null)
        {
            _process = process;
            _complete = complete;
            _canHandle = canHandle;
            _added = added;
            _handle = handle;
        }

        public static string DescribeException(Exception exception) => Describe(exception);

        protected override void ProcessItem(DisposableItem item) => _process(item);

        protected override void ItemComplete(DisposableItem item) => _complete?.Invoke(item);

        protected override void ItemAdded(DisposableItem item) => _added?.Invoke(item);

        protected override void HandleItem(DisposableItem item)
        {
            if (_handle == null)
            {
                base.HandleItem(item);
                return;
            }

            _handle(item);
        }

        protected override bool CanHandleItemNow(DisposableItem item)
            => _canHandle?.Invoke(item) ?? true;
    }

    private sealed class DisposableItem : IDisposable
    {
        public int Value { get; }
        public bool Disposed { get; private set; }

        public DisposableItem(int value) => Value = value;

        public void Dispose() => Disposed = true;
    }

    [Fact]
    public void Queue_processes_items_and_exposes_state()
    {
        using var completed = new CountdownEvent(3);
        var values = new ConcurrentBag<int>();
        using var queue = new ProducerConsumerQueueAction<DisposableItem>(
            item =>
            {
                values.Add(item.Value);
                completed.Signal();
            },
            autoCleanUp: false);

        queue.IsTimed.ShouldBeFalse();
        queue.IsExecuting.ShouldBeFalse();
        queue.AddItems([
            new DisposableItem(1),
            null,
            new DisposableItem(2),
            new DisposableItem(3)
        ]);

        completed.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
        values.OrderBy(value => value).ShouldBe([1, 2, 3]);
        queue.Items.ShouldBeEmpty();
        queue.ExecutingCount.ShouldBe(0);
    }

    [Fact]
    public void Queue_action_supports_timeout_and_comparer_constructor()
    {
        using var completed = new CountdownEvent(2);
        using var queue = new ProducerConsumerQueueAction<DisposableItem>(
            _ => completed.Signal(),
            autoCleanUp: false,
            timeout: TimeSpan.FromMilliseconds(20),
            comparer: Comparer<DisposableItem>.Create((left, right) => right.Value.CompareTo(left.Value)));

        queue.IsTimed.ShouldBeTrue();
        queue.AddItems([new DisposableItem(1), new DisposableItem(2)]);
        completed.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
    }

    [Fact]
    public void Queue_action_default_and_cleanup_constructors_set_expected_state()
    {
        using var defaultQueue = new ProducerConsumerQueueAction<DisposableItem>(_ => { });
        using var cleanupQueue = new ProducerConsumerQueueAction<DisposableItem>(_ => { }, true);

        defaultQueue.AutoCleanupItems.ShouldBeTrue();
        defaultQueue.IsTimed.ShouldBeFalse();
        cleanupQueue.AutoCleanupItems.ShouldBeTrue();
        cleanupQueue.Comparer.ShouldBeNull();
    }

    [Fact]
    public void Queue_rejects_null_single_items_and_ignores_null_batches()
    {
        using var queue = new ProducerConsumerQueueAction<DisposableItem>(_ => { });

        Should.Throw<ArgumentNullException>(() => queue.AddItem(null));
        queue.AddItems(null);
        queue.Clear();
        queue.Items.ShouldBeEmpty();
    }

    [Fact]
    public void Queue_auto_cleanup_disposes_processed_items()
    {
        using var completed = new ManualResetEventSlim();
        var item = new DisposableItem(1);
        using var queue = new ProducerConsumerQueueAction<DisposableItem>(
            _ => completed.Set(),
            autoCleanUp: true);

        queue.AddItem(item);

        completed.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
        SpinWait.SpinUntil(() => item.Disposed, TimeSpan.FromSeconds(5)).ShouldBeTrue();
        item.Disposed.ShouldBeTrue();
    }

    [Fact]
    public void Queue_exposes_executing_items_and_timeout_configuration()
    {
        using var started = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        var item = new DisposableItem(1);
        using var queue = new TestQueue(
            _ =>
            {
                started.Set();
                release.Wait(TimeSpan.FromSeconds(5));
            },
            timeout: TimeSpan.FromMilliseconds(10));

        queue.IsTimed.ShouldBeTrue();
        queue.Timeout.ShouldBe(TimeSpan.FromMilliseconds(10));
        queue.AddItem(item);

        started.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
        queue.IsExecuting.ShouldBeTrue();
        queue.ExecutingCount.ShouldBe(1);
        queue.ExecutingItems.ShouldContain(item);
        release.Set();
    }

    [Fact]
    public void Queue_invokes_item_added_and_completion_hooks()
    {
        using var completed = new ManualResetEventSlim();
        var added = new ConcurrentBag<int>();
        var item = new DisposableItem(1);
        using var queue = new TestQueue(
            _ => { },
            _ => completed.Set(),
            added: queued => added.Add(queued.Value));

        queue.AddItem(item);

        completed.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
        added.ShouldContain(1);
    }

    [Fact]
    public void Queue_can_change_cleanup_setting_and_clear_pending_items()
    {
        using var queue = new TestQueue(
            _ => { },
            autoCleanup: false,
            canHandle: _ => false,
            timeout: TimeSpan.FromMilliseconds(20));
        var item = new DisposableItem(1);

        queue.AutoCleanupItems = true;
        queue.AddItem(item);
        queue.Clear();

        queue.Items.ShouldBeEmpty();
        item.Disposed.ShouldBeFalse();
    }

    [Fact]
    public void Clear_after_dispose_does_not_throw()
    {
        var queue = new TestQueue(
            _ => { },
            autoCleanup: false,
            canHandle: _ => false,
            timeout: TimeSpan.FromMilliseconds(20));

        queue.Dispose();

        Should.NotThrow(() => queue.Clear());
    }

    [Fact]
    public void Queue_can_defer_items_until_the_can_handle_hook_allows_them()
    {
        using var processed = new ManualResetEventSlim();
        var allow = false;
        using var queue = new TestQueue(
            _ => processed.Set(),
            canHandle: _ => allow,
            timeout: TimeSpan.FromMilliseconds(10));

        queue.AddItem(new DisposableItem(1));
        Thread.Sleep(50);
        processed.IsSet.ShouldBeFalse();
        queue.Items.ShouldHaveSingleItem();

        allow = true;
        queue.AddItem(new DisposableItem(2));
        processed.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
    }

    [Fact]
    public void Queue_continues_when_process_and_complete_hooks_throw()
    {
        using var completed = new ManualResetEventSlim();
        using var queue = new TestQueue(
            _ => throw new InvalidOperationException("process"),
            _ =>
            {
                completed.Set();
                throw new InvalidOperationException("complete");
            });

        queue.AddItem(new DisposableItem(1));

        completed.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
        TestQueue.DescribeException(new InvalidOperationException("message")).ShouldContain("message");
        TestQueue.DescribeException(null).ShouldBeEmpty();
    }

    [Fact]
    public void Queue_continues_when_handle_item_throws()
    {
        using var secondItemHandled = new ManualResetEventSlim();
        var handledItems = 0;
        using var queue = new TestQueue(
            _ => { },
            handle: _ =>
            {
                if (Interlocked.Increment(ref handledItems) == 1)
                {
                    throw new InvalidOperationException("handle");
                }

                secondItemHandled.Set();
            });

        queue.AddItems([new DisposableItem(1), new DisposableItem(2)]);

        secondItemHandled.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
        handledItems.ShouldBe(2);
    }
}
