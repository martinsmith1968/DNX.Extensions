using DNX.Extensions.Threading;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Threading;

public class ThreadHelperTests
{
    [Fact]
    public void Start_runs_parameterized_delegate()
    {
        using var completed = new ManualResetEventSlim();
        var result = 0;
        var isThreadPoolThread = true;

        var thread = ThreadHelper.Start<int>(value =>
        {
            result = value;
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread;
            completed.Set();
        }, 42);

        completed.Wait(TimeSpan.FromSeconds(5)).ShouldBeTrue();
        thread.Join(TimeSpan.FromSeconds(5)).ShouldBeTrue();
        result.ShouldBe(42);
        isThreadPoolThread.ShouldBeFalse();
    }

    [Fact]
    public void Start_runs_thread_start_delegate()
    {
        using var completed = new ManualResetEventSlim();

        var thread = ThreadHelper.Start(() => completed.Set());

        completed.Wait(TimeSpan.FromSeconds(5)).ShouldBeTrue();
        thread.Join(TimeSpan.FromSeconds(5)).ShouldBeTrue();
    }

    [Fact]
    public void StartOnThreadPool_runs_parameterized_delegate()
    {
        using var completed = new ManualResetEventSlim();
        var result = 0;
        var isThreadPoolThread = false;

        ThreadHelper.StartOnThreadPool<int>(value =>
        {
            result = value;
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread;
            completed.Set();
        }, 24).ShouldBeTrue();

        completed.Wait(TimeSpan.FromSeconds(5)).ShouldBeTrue();
        result.ShouldBe(24);
        isThreadPoolThread.ShouldBeTrue();
    }

    [Fact]
    public void StartOnThreadPool_runs_thread_start_delegate()
    {
        using var completed = new ManualResetEventSlim();

        ThreadHelper.StartOnThreadPool(() => completed.Set()).ShouldBeTrue();

        completed.Wait(TimeSpan.FromSeconds(5)).ShouldBeTrue();
    }

    [Fact]
    public void ThreadHelper_rejects_null_delegates()
    {
        Should.Throw<ArgumentNullException>(() => ThreadHelper.Start<int>(null, 1));
        Should.Throw<ArgumentNullException>(() => ThreadHelper.Start((ThreadStart)null));
        Should.Throw<ArgumentNullException>(() => ThreadHelper.StartOnThreadPool<int>(null, 1));
        Should.Throw<ArgumentNullException>(() => ThreadHelper.StartOnThreadPool((ThreadStart)null));
    }
}
