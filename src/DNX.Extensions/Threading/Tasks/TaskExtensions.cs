using System;
using System.Collections.Generic;
using System.Linq;
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
    /// complete within the given timeout. The task itself is not cancelled - it continues
    /// running in the background if the timeout elapses first.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="timeout">The timeout.</param>
    /// <exception cref="TimeoutException">The task did not complete within the timeout.</exception>
    public static async Task WithTimeout(this Task task, TimeSpan timeout)
    {
        Guard.IsNotNull(() => task);

        var timeoutTask = Task.Delay(timeout);

        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);
        if (completedTask == timeoutTask)
        {
            throw new TimeoutException($"The operation did not complete within {timeout}");
        }

        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits the specified task, throwing a <see cref="TimeoutException"/> if it does not
    /// complete within the given timeout. The task itself is not cancelled - it continues
    /// running in the background if the timeout elapses first.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task">The task.</param>
    /// <param name="timeout">The timeout.</param>
    /// <returns>The result of the task.</returns>
    /// <exception cref="TimeoutException">The task did not complete within the timeout.</exception>
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
    {
        Guard.IsNotNull(() => task);

        var timeoutTask = Task.Delay(timeout);

        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);
        if (completedTask == timeoutTask)
        {
            throw new TimeoutException($"The operation did not complete within {timeout}");
        }

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

        _ = RunSafely.ExecuteAsync(task, exceptionHandler);
    }

    /// <summary>
    /// Awaits all the specified tasks, and if any fail, throws an <see cref="AggregateException"/>
    /// containing every exception raised, rather than only the first one as a plain
    /// <c>await Task.WhenAll(...)</c> would surface.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    public static async Task WhenAllOrThrow(this IEnumerable<Task> tasks)
    {
        var taskList = tasks?.ToList();

        Guard.IsNotNullOrEmpty(() => taskList);

        var whenAllTask = Task.WhenAll(taskList);

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
    /// <c>await Task.WhenAll(...)</c> would surface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tasks">The tasks.</param>
    /// <returns>The results of each task, in the order the tasks were supplied.</returns>
    public static async Task<T[]> WhenAllOrThrow<T>(this IEnumerable<Task<T>> tasks)
    {
        var taskList = tasks?.ToList();

        Guard.IsNotNullOrEmpty(() => taskList);

        var whenAllTask = Task.WhenAll(taskList);

        try
        {
            return await whenAllTask.ConfigureAwait(false);
        }
        catch when (whenAllTask.Exception != null)
        {
            throw whenAllTask.Exception;
        }
    }
}
