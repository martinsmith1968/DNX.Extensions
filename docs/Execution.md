# DNX.Extensions.Execution

Two complementary helpers for executing code that might fail: `RunSafely` runs code without
`try`/`catch` boilerplate, swallowing (and optionally reporting) any exception; `Retry`
re-attempts code that may fail transiently, with configurable backoff between attempts.

## RunSafely class

Executes an `Action`/`Func<T>`/`Task`/`Task<T>` and swallows any exception it throws, optionally
passing it to a supplied `exceptionHandler`. If `exceptionHandler` is omitted the exception is
simply discarded. The handler itself is invoked defensively - if `exceptionHandler` throws, that
is traced via `Trace.WriteLine` rather than propagated.

| Method Name | Parameters | Description |
| --- | --- | --- |
| Execute | `Action` action, `Action<Exception>` exceptionHandler = null | Runs `action`; on failure, invokes `exceptionHandler` (if supplied) and returns |
| Execute\<T\> | `Func<T>` func, `Action<Exception>` exceptionHandler = null | Runs `func` and returns its result; on failure, invokes `exceptionHandler` and returns `default(T)` |
| Execute\<T\> | `Func<T>` func, `T` defaultValue, `Action<Exception>` exceptionHandler = null | As above, returning `defaultValue` instead of `default(T)` on failure |
| ExecuteAsync | `Task` task, `Action<Exception>` exceptionHandler = null | Awaits `task`; on failure, invokes `exceptionHandler` and returns |
| ExecuteAsync\<T\> | `Task<T>` task, `Action<Exception>` exceptionHandler = null | Awaits `task` and returns its result; on failure, invokes `exceptionHandler` and returns `default(T)` |
| ExecuteAsync\<T\> | `Task<T>` task, `T` defaultValue, `Action<Exception>` exceptionHandler = null | As above, returning `defaultValue` instead of `default(T)` on failure |

**Cancellation is not routed through `exceptionHandler`.** If the awaited task throws
`OperationCanceledException`, both `ExecuteAsync` overloads let it propagate unchanged instead of
passing it to `exceptionHandler` and swallowing it. Cancellation is an intentional outcome that
the caller itself requested (via its `CancellationToken`), not a fault in the code being run, so
treating it as a reported "failure" would be misleading - code awaiting `ExecuteAsync` still sees
the cancellation the normal way, via `await`/`catch (OperationCanceledException)`.

Example:

```csharp
// Execute - exception swallowed, no handler supplied
RunSafely.Execute(() => throw new Exception("boom"));
// Returns normally - nothing thrown, nothing reported

// Execute<T> - divide by zero returns default(int)
var dividend = 1000;
var divisor = 0;
var value = RunSafely.Execute(() => dividend / divisor);
// value == 0

// Execute<T> with an explicit default and an exception handler
var message = "";
var value2 = RunSafely.Execute(() => dividend / divisor, 500, ex => message = ex.Message);
// value2 == 500, message contains "divide by zero"

// ExecuteAsync - cancellation propagates instead of going through exceptionHandler
using var cts = new CancellationTokenSource();
cts.Cancel();
var cancelledTask = Task.FromCanceled(cts.Token);
Exception caught = null;

await RunSafely.ExecuteAsync(cancelledTask, ex => caught = ex);
// Throws OperationCanceledException - caught stays null, the handler is never invoked
```

## Retry class

Re-attempts an `Action`/`Func<T>`/`Func<Task>`/`Func<Task<T>>` that may fail transiently, up to
`maxAttempts` times, waiting between attempts according to a `delayProvider`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| Execute | `Action` action, `int` maxAttempts = 3, `Func<int, TimeSpan>` delayProvider = null, `Func<Exception, bool>` shouldRetry = null, `Action<Exception, int>` onRetry = null, `CancellationToken` cancellationToken = default | Runs `action`, retrying on failure as described below |
| Execute\<T\> | `Func<T>` func, same remaining parameters | As above, returning `func`'s result |
| ExecuteAsync | `Func<Task>` taskFactory, same remaining parameters | As `Execute`, but asynchronous; `taskFactory` creates a new `Task` for each attempt, since a `Task` cannot be re-run |
| ExecuteAsync\<T\> | `Func<Task<T>>` taskFactory, same remaining parameters | As above, returning the task's result |

Behaviour shared by all four methods:

- `maxAttempts` counts the **first** attempt, not just the retries - `maxAttempts: 3` means one
  initial attempt plus up to two retries.
- `delayProvider` is given the attempt number that just failed (1-based) and returns the
  `TimeSpan` to wait before the next attempt. If omitted, it defaults to exponential backoff
  starting at 100ms and doubling each attempt (the same formula as
  `DelayProviders.MillisecondExponentialBackoff(100)`).
- `shouldRetry` is given the exception from a failed attempt and decides whether it's worth
  retrying; if omitted, every exception is retried.
- `onRetry` is invoked after a failed attempt that is about to be retried (not after the final,
  unretried failure), with the exception and the attempt number that failed.
- `OperationCanceledException` is never retried, regardless of `shouldRetry` - it always
  propagates immediately.
- `cancellationToken` is checked before every attempt, and is also honoured **during the
  inter-attempt delay itself** (`CancellationToken.WaitHandle.WaitOne(delay)` for the synchronous
  overloads, `Task.Delay(delay, cancellationToken)` for the async ones) - so a cancellation
  request doesn't have to wait out a long backoff delay before taking effect.
- If every attempt fails, the exception from the **final** attempt is rethrown.

### Retry.DelayProviders static class

Factory methods that build a `Func<int, TimeSpan>` suitable for passing as `delayProvider`.

| Static Method | Parameters | Description |
| --- | --- | --- |
| MillisecondExponentialBackoff | int milliseconds | Returns a provider where attempt `n` delays `milliseconds * 2^(n-1)` |
| MillisecondLinearBackoff | int milliseconds | Returns a provider where attempt `n` delays `milliseconds * n` |
| SecondsExponentialBackoff | int seconds | Returns a provider where attempt `n` delays `seconds * 2^(n-1)`, as a `TimeSpan` of seconds |
| SecondsLinearBackoff | int seconds | Returns a provider where attempt `n` delays `seconds * n`, as a `TimeSpan` of seconds |

The internal default delay provider used when `delayProvider` is omitted is exponential backoff
based on 100ms - equivalent to `DelayProviders.MillisecondExponentialBackoff(100)`.

Example:

```csharp
var attempts = 0;

Retry.Execute(
    () =>
    {
        ++attempts;
        if (attempts < 3)
        {
            throw new InvalidOperationException("not yet");
        }
    },
    maxAttempts: 5,
    delayProvider: _ => TimeSpan.Zero);

// attempts == 3 - succeeded on the third try, no further attempts made

// A DelayProviders factory can be passed straight in as delayProvider
await Retry.ExecuteAsync(
    async () =>
    {
        ++attempts;
        if (attempts < 2)
        {
            throw new InvalidOperationException("not yet");
        }

        await Task.CompletedTask;
    },
    delayProvider: Retry.DelayProviders.MillisecondLinearBackoff(1));
// attempts == 2

// shouldRetry excludes a particular exception type from being retried at all
Retry.Execute(
    () => throw new ArgumentException("not retryable"),
    maxAttempts: 5,
    delayProvider: _ => TimeSpan.Zero,
    shouldRetry: ex => ex is not ArgumentException);
// Throws ArgumentException after a single attempt - it was never eligible for retry

// Default backoff, in milliseconds, for each successive failed attempt:
// attempt 1 -> 100ms, attempt 2 -> 200ms, attempt 3 -> 400ms, attempt 4 -> 800ms
```
