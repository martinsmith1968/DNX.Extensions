using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNX.Extensions.Execution;
using DNX.Extensions.Validation.BuiltInTypes;

namespace DNX.Extensions.Threading.Tasks;

/// <summary>
/// Task Extensions
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Awaits the specified task, throwing a <see cref="TimeoutException"/> if it does not
    /// complete within the given timeout, or an <see cref="OperationCanceledException"/> if
    /// <paramref name="cancellationToken"/> is cancelled first. The task itself is not
    /// cancelled in either case - it continues running in the background.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="timeout">The timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="TimeoutException">The task did not complete within the timeout.</exception>
    public static async Task WithTimeout(this Task task, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(() => task);

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var timeoutTask = Task.Delay(timeout, timeoutCts.Token);

        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);
        if (completedTask == timeoutTask)
        {
            cancellationToken.ThrowIfCancellationRequested();

            throw new TimeoutException($"The operation did not complete within {timeout}");
        }

        // The real task won the race - stop the now-redundant delay/timer promptly.
        timeoutCts.Cancel();

        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits the specified task, throwing a <see cref="TimeoutException"/> if it does not
    /// complete within the given timeout, or an <see cref="OperationCanceledException"/> if
    /// <paramref name="cancellationToken"/> is cancelled first. The task itself is not
    /// cancelled in either case - it continues running in the background.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task">The task.</param>
    /// <param name="timeout">The timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the task.</returns>
    /// <exception cref="TimeoutException">The task did not complete within the timeout.</exception>
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(() => task);

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var timeoutTask = Task.Delay(timeout, timeoutCts.Token);

        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);
        if (completedTask == timeoutTask)
        {
            cancellationToken.ThrowIfCancellationRequested();

            throw new TimeoutException($"The operation did not complete within {timeout}");
        }

        // The real task won the race - stop the now-redundant delay/timer promptly.
        timeoutCts.Cancel();

        return await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Runs the specified task without awaiting it, observing any exception it throws so it
    /// does not go unhandled. Use this for intentional fire-and-forget work.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="exceptionHandler">The exception handler.</param>
    public static void FireAndForget(this Task task, Action<Exception> exceptionHandler = null)
    {
        Guard.IsNotNull(() => task);

        _ = FireAndForgetAsync(task, exceptionHandler);
    }

    private static async Task FireAndForgetAsync(Task task, Action<Exception> exceptionHandler)
    {
        try
        {
            await RunSafely.ExecuteAsync(task, exceptionHandler).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // RunSafely deliberately lets cancellation propagate rather than swallowing it
            // like a fault - here there is no awaiter to propagate it to, so it simply ends
            // the fire-and-forget operation without being reported as an error.
        }
    }

    /// <summary>
    /// Awaits all the specified tasks, and if any fail, throws an <see cref="AggregateException"/>
    /// containing every exception raised, rather than only the first one as a plain
    /// <c>await Task.WhenAll(...)</c> would surface. The tasks themselves are not cancelled if
    /// <paramref name="cancellationToken"/> is cancelled first - they continue running in the
    /// background.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static async Task WhenAllOrThrow(this IEnumerable<Task> tasks, CancellationToken cancellationToken = default)
    {
        var taskList = tasks?.ToList();

        Guard.IsNotNullOrEmpty(() => taskList);

        var whenAllTask = Task.WhenAll(taskList);

        await WaitForCancellableCompletion(whenAllTask, cancellationToken).ConfigureAwait(false);

        try
        {
            await whenAllTask.ConfigureAwait(false);
        }
        catch when (whenAllTask.Exception != null)
        {
            throw whenAllTask.Exception;
        }
    }

    /// <summary>
    /// Awaits all the specified tasks, and if any fail, throws an <see cref="AggregateException"/>
    /// containing every exception raised, rather than only the first one as a plain
    /// <c>await Task.WhenAll(...)</c> would surface. The tasks themselves are not cancelled if
    /// <paramref name="cancellationToken"/> is cancelled first - they continue running in the
    /// background.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tasks">The tasks.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The results of each task, in the order the tasks were supplied.</returns>
    public static async Task<T[]> WhenAllOrThrow<T>(this IEnumerable<Task<T>> tasks, CancellationToken cancellationToken = default)
    {
        var taskList = tasks?.ToList();

        Guard.IsNotNullOrEmpty(() => taskList);

        var whenAllTask = Task.WhenAll(taskList);

        await WaitForCancellableCompletion(whenAllTask, cancellationToken).ConfigureAwait(false);

        try
        {
            return await whenAllTask.ConfigureAwait(false);
        }
        catch when (whenAllTask.Exception != null)
        {
            throw whenAllTask.Exception;
        }
    }

    /// <summary>
    /// Waits for the task to complete, throwing an <see cref="OperationCanceledException"/>
    /// immediately if the cancellation token is cancelled first.
    /// </summary>
    private static async Task WaitForCancellableCompletion(Task task, CancellationToken cancellationToken)
    {
        if (!cancellationToken.CanBeCanceled)
        {
            return;
        }

        var cancellationTask = Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);

        var completedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);
        if (completedTask == cancellationTask)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
