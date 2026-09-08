using System;
using DNX.Extensions.Threading.Mutexes;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Threading;

public class MutexTests
{
    [Fact]
    public void Mutex_acquires_and_releases_named_lock()
    {
        var name = Guid.NewGuid().ToString();
        var before = DateTime.UtcNow;
        MutexManager.CanAcquire(name).ShouldBeTrue();

        using (var mutex = MutexManager.Acquire(name))
        {
            mutex.Name.ShouldBe(name);
            mutex.State.ShouldBe(MutexState.Acquired);
            mutex.LockObject.ShouldNotBeNull();
            mutex.ThreadId.ShouldBe(Environment.CurrentManagedThreadId);
            mutex.Timestamp.ShouldBeInRange(before, DateTime.UtcNow);
            MutexManager.CanAcquire(name).ShouldBeFalse();
            MutexManager.AcquireNoWait(name).ShouldBeNull();
        }

        MutexManager.CanAcquire(name).ShouldBeTrue();
    }

    [Fact]
    public void AcquireNoWait_returns_mutex_when_name_is_available()
    {
        var name = Guid.NewGuid().ToString();

        using var mutex = MutexManager.AcquireNoWait(name);

        mutex.ShouldNotBeNull();
        mutex.State.ShouldBe(MutexState.Acquired);
    }

    [Fact]
    public void Mutex_dispose_marks_lock_released()
    {
        var mutex = MutexManager.Acquire(Guid.NewGuid().ToString());

        mutex.Dispose();

        mutex.State.ShouldBe(MutexState.Released);
    }

    [Fact]
    public void Public_mutex_constructor_acquires_named_lock()
    {
        var name = Guid.NewGuid().ToString();

        using var mutex = new DNX.Extensions.Threading.Mutexes.Mutex(name);

        mutex.Name.ShouldBe(name);
        mutex.State.ShouldBe(MutexState.Acquired);
        MutexManager.Mutexes[name].ShouldBeSameAs(mutex);
    }
}
