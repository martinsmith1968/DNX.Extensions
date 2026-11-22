using System;
using System.Threading;
using System.Threading.Tasks;
using DNX.Extensions.Validation.BuiltInTypes;

namespace DNX.Extensions.Execution;

/// <summary>
/// Retry code that may fail transiently, with configurable backoff between attempts.
/// </summary>
public static class Retry
{
    public static class DelayProviders
    {
        public static Func<int, TimeSpan> MillisecondExponentialBackoff(int milliseconds)
            => attempt => MillisecondExponentialBackoffDelayProvider(attempt, milliseconds);
        public static Func<int, TimeSpan> MillisecondLinearBackoff(int milliseconds)
            => attempt => MillisecondLinearBackoffDelayProvider(attempt, milliseconds);
        public static Func<int, TimeSpan> SecondsExponentialBackoff(int seconds)
            => attempt => SecondsExponentialBackoffDelayProvider(attempt, seconds);
        public static Func<int, TimeSpan> SecondsLinearBackoff(int seconds)
            => attempt => SecondsLinearBackoffDelayProvider(attempt, seconds);
    }

    /// <summary>
    /// The default delay provider - exponential backoff starting at 100ms and doubling each attempt.
    /// </summary>
    /// <remarks>Internal (rather than private) so its exact formula can be unit tested directly.</remarks>
    internal static TimeSpan DefaultDelayProvider(int attempt)
        => MillisecondExponentialBackoffDelayProvider(attempt, 100);

    internal static TimeSpan MillisecondExponentialBackoffDelayProvider(int attempt, int milliseconds)
    {
        return TimeSpan.FromMilliseconds(milliseconds * Math.Pow(2, attempt - 1));
    }

    internal static TimeSpan MillisecondLinearBackoffDelayProvider(int attempt, int milliseconds)
    {
        return TimeSpan.FromMilliseconds(milliseconds * attempt);
    }

    internal static TimeSpan SecondsExponentialBackoffDelayProvider(int attempt, int seconds)
    {
        return TimeSpan.FromSeconds(seconds * Math.Pow(2, attempt - 1));
    }

    internal static TimeSpan SecondsLinearBackoffDelayProvider(int attempt, int seconds)
    {
        return TimeSpan.FromSeconds(seconds * attempt);
    }

    /// <summary>
    /// Executes the specified action, retrying on failure.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="maxAttempts">The maximum number of attempts, including the first.</param>
    /// <param name="delayProvider">Given the attempt number that just failed (1-based), returns the delay before the next attempt. Defaults to exponential backoff.</param>
    /// <param name="shouldRetry">Determines whether a given exception should be retried. Defaults to retrying every exception.</param>
    /// <param name="onRetry">Invoked after a failed attempt that will be retried, with the exception and the attempt number that failed.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="Exception">The exception from the final attempt, if every attempt fails.</exception>
    public static void Execute(
        Action action,
        int maxAttempts = 3,
        Func<int, TimeSpan> delayProvider = null,
        Func<Exception, bool> shouldRetry = null,
        Action<Exception, int> onRetry = null,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(() => action);

        Execute(
            () =>
            {
                action();
                return true;
            },
            maxAttempts, delayProvider, shouldRetry, onRetry, cancellationToken);
    }

    /// <summary>
    /// Executes the specified function, retrying on failure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func">The function.</param>
    /// <param name="maxAttempts">The maximum number of attempts, including the first.</param>
    /// <param name="delayProvider">Given the attempt number that just failed (1-based), returns the delay before the next attempt. Defaults to exponential backoff.</param>
    /// <param name="shouldRetry">Determines whether a given exception should be retried. Defaults to retrying every exception.</param>
    /// <param name="onRetry">Invoked after a failed attempt that will be retried, with the exception and the attempt number that failed.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>T.</returns>
    /// <exception cref="Exception">The exception from the final attempt, if every attempt fails.</exception>
    public static T Execute<T>(
        Func<T> func,
        int maxAttempts = 3,
        Func<int, TimeSpan> delayProvider = null,
        Func<Exception, bool> shouldRetry = null,
        Action<Exception, int> onRetry = null,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(() => func);
        Guard.IsGreaterThan(() => maxAttempts, 0);

        delayProvider ??= DefaultDelayProvider;

        for (var attempt = 1; ; ++attempt)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                return func();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex) when (attempt < maxAttempts && (shouldRetry?.Invoke(ex) ?? true))
            {
                onRetry?.Invoke(ex, attempt);

                var delay = delayProvider(attempt);
                if (delay > TimeSpan.Zero)
                {
                    cancellationToken.WaitHandle.WaitOne(delay);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }
    }

    /// <summary>
    /// execute as an asynchronous operation, retrying on failure.
    /// </summary>
    /// <param name="taskFactory">Creates a new task for each attempt - a task cannot be re-run, so a fresh one is needed per attempt.</param>
    /// <param name="maxAttempts">The maximum number of attempts, including the first.</param>
    /// <param name="delayProvider">Given the attempt number that just failed (1-based), returns the delay before the next attempt. Defaults to exponential backoff.</param>
    /// <param name="shouldRetry">Determines whether a given exception should be retried. Defaults to retrying every exception.</param>
    /// <param name="onRetry">Invoked after a failed attempt that will be retried, with the exception and the attempt number that failed.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="Exception">The exception from the final attempt, if every attempt fails.</exception>
    public static async Task ExecuteAsync(
        Func<Task> taskFactory,
        int maxAttempts = 3,
        Func<int, TimeSpan> delayProvider = null,
        Func<Exception, bool> shouldRetry = null,
        Action<Exception, int> onRetry = null,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(() => taskFactory);

        await ExecuteAsync(
            async () =>
            {
                await taskFactory().ConfigureAwait(false);
                return true;
            },
            maxAttempts, delayProvider, shouldRetry, onRetry, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// execute as an asynchronous operation, retrying on failure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="taskFactory">Creates a new task for each attempt - a task cannot be re-run, so a fresh one is needed per attempt.</param>
    /// <param name="maxAttempts">The maximum number of attempts, including the first.</param>
    /// <param name="delayProvider">Given the attempt number that just failed (1-based), returns the delay before the next attempt. Defaults to exponential backoff.</param>
    /// <param name="shouldRetry">Determines whether a given exception should be retried. Defaults to retrying every exception.</param>
    /// <param name="onRetry">Invoked after a failed attempt that will be retried, with the exception and the attempt number that failed.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;T&gt;.</returns>
    /// <exception cref="Exception">The exception from the final attempt, if every attempt fails.</exception>
    public static async Task<T> ExecuteAsync<T>(
        Func<Task<T>> taskFactory,
        int maxAttempts = 3,
        Func<int, TimeSpan> delayProvider = null,
        Func<Exception, bool> shouldRetry = null,
        Action<Exception, int> onRetry = null,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(() => taskFactory);
        Guard.IsGreaterThan(() => maxAttempts, 0);

        delayProvider ??= DefaultDelayProvider;

        for (var attempt = 1; ; ++attempt)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex) when (attempt < maxAttempts && (shouldRetry?.Invoke(ex) ?? true))
            {
                onRetry?.Invoke(ex, attempt);

                var delay = delayProvider(attempt);
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
