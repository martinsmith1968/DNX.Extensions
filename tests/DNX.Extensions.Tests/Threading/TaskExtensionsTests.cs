using DNX.Extensions.Threading.Tasks;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Threading;

public class TaskExtensionsTests
{
    public class WithTimeout_Tests
    {
        [Fact]
        public async Task Returns_normally_when_task_completes_within_timeout()
        {
            var task = Task.Delay(TimeSpan.FromMilliseconds(10), TestContext.Current.CancellationToken);

            await task.WithTimeout(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        }

        [Fact]
        public async Task Throws_TimeoutException_when_task_does_not_complete_in_time()
        {
            using var neverCompletes = new ManualResetEventSlim();
            var task = Task.Run(
                () => neverCompletes.Wait(TimeSpan.FromSeconds(30), TestContext.Current.CancellationToken),
                TestContext.Current.CancellationToken);

            await Should.ThrowAsync<TimeoutException>(() => task.WithTimeout(TimeSpan.FromMilliseconds(10)));
        }

        [Fact]
        public async Task Propagates_the_original_exception_when_the_task_fails_before_the_timeout()
        {
            var task = Task.Run(() => throw new InvalidOperationException("boom"), TestContext.Current.CancellationToken);

            await Should.ThrowAsync<InvalidOperationException>(() => task.WithTimeout(TimeSpan.FromSeconds(5)));
        }

        [Fact]
        public async Task Throws_OperationCanceledException_rather_than_TimeoutException_when_the_token_is_cancelled_first()
        {
            using var neverCompletes = new ManualResetEventSlim();
            var task = Task.Run(
                () => neverCompletes.Wait(TimeSpan.FromSeconds(30), TestContext.Current.CancellationToken),
                TestContext.Current.CancellationToken);

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(10));

            await Should.ThrowAsync<OperationCanceledException>(() => task.WithTimeout(TimeSpan.FromSeconds(30), cts.Token));
        }
    }

    public class WithTimeoutT_Tests
    {
        [Fact]
        public async Task Returns_the_result_when_task_completes_within_timeout()
        {
            var task = Task.FromResult(42);

            var result = await task.WithTimeout(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);

            result.ShouldBe(42);
        }

        [Fact]
        public async Task Throws_TimeoutException_when_task_does_not_complete_in_time()
        {
            using var neverCompletes = new ManualResetEventSlim();
            var task = Task.Run(() =>
            {
                neverCompletes.Wait(TimeSpan.FromSeconds(30), TestContext.Current.CancellationToken);
                return 42;
            }, TestContext.Current.CancellationToken);

            await Should.ThrowAsync<TimeoutException>(() => task.WithTimeout(TimeSpan.FromMilliseconds(10)));
        }
    }

    public class FireAndForget_Tests
    {
        [Fact]
        public void Runs_the_task_to_completion_without_being_awaited()
        {
            using var completed = new ManualResetEventSlim();
            var task = Task.Run(() => completed.Set(), TestContext.Current.CancellationToken);

            task.FireAndForget();

            completed.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
        }

        [Fact]
        public void Routes_a_faulted_task_through_the_exception_handler_instead_of_throwing()
        {
            using var handled = new ManualResetEventSlim();
            Exception caught = null;

            var task = Task.Run(() => throw new InvalidOperationException("boom"), TestContext.Current.CancellationToken);

            task.FireAndForget(ex =>
            {
                caught = ex;
                handled.Set();
            });

            handled.Wait(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ShouldBeTrue();
            caught.ShouldBeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task Swallows_a_cancelled_task_without_invoking_the_exception_handler()
        {
            Exception caught = null;

            using var cts = new CancellationTokenSource();
            cts.Cancel();
            var task = Task.FromCanceled(cts.Token);

            task.FireAndForget(ex => caught = ex);

            // There is nothing to await (that is the point of fire-and-forget), so give the
            // background continuation a moment to run before asserting nothing was reported.
            await Task.Delay(TimeSpan.FromMilliseconds(50), TestContext.Current.CancellationToken);

            caught.ShouldBeNull();
        }
    }

    public class WhenAllOrThrow_Tests
    {
        [Fact]
        public async Task Completes_normally_when_every_task_succeeds()
        {
            var cancellationToken = TestContext.Current.CancellationToken;
            var tasks = new[] { Task.Delay(1, cancellationToken), Task.Delay(1, cancellationToken), Task.Delay(1, cancellationToken) };

            await tasks.WhenAllOrThrow(cancellationToken);
        }

        [Fact]
        public async Task Throws_an_AggregateException_containing_every_failure()
        {
            var cancellationToken = TestContext.Current.CancellationToken;
            var tasks = new[]
            {
                Task.Run(() => throw new InvalidOperationException("first"), cancellationToken),
                Task.Run(() => throw new ArgumentException("second"), cancellationToken),
            };

            var ex = await Should.ThrowAsync<AggregateException>(() => tasks.WhenAllOrThrow());

            ex.InnerExceptions.Count.ShouldBe(2);
            ex.InnerExceptions.ShouldContain(e => e is InvalidOperationException);
            ex.InnerExceptions.ShouldContain(e => e is ArgumentException);
        }

        [Fact]
        public async Task Throws_OperationCanceledException_when_the_token_is_cancelled_before_completion()
        {
            using var neverCompletes = new ManualResetEventSlim();
            var tasks = new[]
            {
                Task.Run(
                    () => neverCompletes.Wait(TimeSpan.FromSeconds(30), TestContext.Current.CancellationToken),
                    TestContext.Current.CancellationToken),
            };

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(10));

            await Should.ThrowAsync<OperationCanceledException>(() => tasks.WhenAllOrThrow(cts.Token));
        }
    }

    public class WhenAllOrThrowT_Tests
    {
        [Fact]
        public async Task Returns_the_results_in_order_when_every_task_succeeds()
        {
            var tasks = new[] { Task.FromResult(1), Task.FromResult(2), Task.FromResult(3) };

            var results = await tasks.WhenAllOrThrow(TestContext.Current.CancellationToken);

            results.ShouldBe([1, 2, 3]);
        }

        [Fact]
        public async Task Throws_an_AggregateException_containing_every_failure()
        {
            var cancellationToken = TestContext.Current.CancellationToken;
            var tasks = new[]
            {
                Task.Run((Func<int>)(() => throw new InvalidOperationException("first")), cancellationToken),
                Task.Run((Func<int>)(() => throw new ArgumentException("second")), cancellationToken),
            };

            var ex = await Should.ThrowAsync<AggregateException>(() => tasks.WhenAllOrThrow());

            ex.InnerExceptions.Count.ShouldBe(2);
        }
    }
}
