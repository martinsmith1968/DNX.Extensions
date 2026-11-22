using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DNX.Extensions.Execution;

/// <summary>
/// Execute code without having to implement try...catch
/// </summary>
public class RunSafely
{
    /// <summary>
    /// Invokes the exception handler, tracing (rather than silently discarding) any exception
    /// the handler itself throws.
    /// </summary>
    private static void InvokeExceptionHandler(Action<Exception> exceptionHandler, Exception ex)
    {
        try
        {
            exceptionHandler?.Invoke(ex);
        }
        catch (Exception handlerEx)
        {
            Trace.WriteLine($"RunSafely: exceptionHandler itself threw: {handlerEx}");
        }
    }

    /// <summary>
    /// Executes the specified action.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="exceptionHandler">The exception handler.</param>
    public static void Execute(Action action, Action<Exception> exceptionHandler = null)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            InvokeExceptionHandler(exceptionHandler, ex);
        }
    }

    /// <summary>
    /// Executes the specified function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func">The function.</param>
    /// <param name="exceptionHandler">The exception handler.</param>
    /// <returns>T.</returns>
    public static T Execute<T>(Func<T> func, Action<Exception> exceptionHandler = null)
    {
        return Execute(func, default, exceptionHandler);
    }

    /// <summary>
    /// Executes the specified function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func">The function.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="exceptionHandler">The exception handler.</param>
    /// <returns>T.</returns>
    public static T Execute<T>(Func<T> func, T defaultValue, Action<Exception> exceptionHandler = null)
    {
        try
        {
            return func.Invoke();
        }
        catch (Exception ex)
        {
            InvokeExceptionHandler(exceptionHandler, ex);

            return defaultValue;
        }
    }

    /// <summary>
    /// execute as an asynchronous operation.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="exceptionHandler">The exception handler.</param>
    public static async Task ExecuteAsync(Task task, Action<Exception> exceptionHandler = null)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Cancellation is an intentional outcome, not a fault - let it propagate rather
            // than routing it through exceptionHandler like a genuine failure.
            throw;
        }
        catch (Exception ex)
        {
            InvokeExceptionHandler(exceptionHandler, ex);
        }
    }

    /// <summary>
    /// execute as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task">The task.</param>
    /// <param name="exceptionHandler">The exception handler.</param>
    /// <returns>Task&lt;T&gt;.</returns>
    public static async Task<T> ExecuteAsync<T>(Task<T> task, Action<Exception> exceptionHandler = null)
    {
        return await ExecuteAsync(task, default, exceptionHandler);
    }

    /// <summary>
    /// execute as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task">The task.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="exceptionHandler">The exception handler.</param>
    /// <returns>Task&lt;T&gt;.</returns>
    public static async Task<T> ExecuteAsync<T>(Task<T> task, T defaultValue, Action<Exception> exceptionHandler = null)
    {
        try
        {
            return await task.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Cancellation is an intentional outcome, not a fault - let it propagate rather
            // than routing it through exceptionHandler like a genuine failure.
            throw;
        }
        catch (Exception ex)
        {
            InvokeExceptionHandler(exceptionHandler, ex);

            return defaultValue;
        }
    }
}
