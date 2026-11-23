# DNX.Extensions.Threading

Helpers for working with threads, named cross-thread locks, a producer/consumer processing
queue, and `Task` extensions for timeouts, fire-and-forget, and aggregate-failure awaiting.

## ThreadHelper class

Starts threads (or queues work on the thread pool) using either a plain `ThreadStart` or a
generic, strongly-typed `ParameterizedThreadStart<T>` delegate - avoiding the need to box a
parameter into an `object` the way `System.Threading.Thread`'s own parameterized start does.

| Static Method | Parameters | Description |
| --- | --- | --- |
| Start\<T\> | `ParameterizedThreadStart<T>` start, `T` value | Creates and starts a new `Thread` which calls `start(value)`; returns the `Thread` |
| Start | `ThreadStart` start | Creates and starts a new `Thread` running the delegate; returns the `Thread` |
| StartOnThreadPool\<T\> | `ParameterizedThreadStart<T>` start, `T` value | Queues `start(value)` onto the `ThreadPool` via `QueueUserWorkItem`; returns `true` if queued successfully |
| StartOnThreadPool | `ThreadStart` start | Queues the delegate onto the `ThreadPool` via `QueueUserWorkItem`; returns `true` if queued successfully |

All four methods throw `ArgumentNullException` if the delegate passed is `null`.

Example:

```csharp
var thread = ThreadHelper.Start<int>(value =>
{
    Console.WriteLine($"Value: {value}, ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}");
}, 42);

thread.Join();
// Runs on a dedicated (non-pool) thread - "ThreadPool: False"

var queued = ThreadHelper.StartOnThreadPool<int>(value =>
{
    Console.WriteLine($"Value: {value}, ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}");
}, 24);
// queued == true, and the delegate runs on a pool thread - "ThreadPool: True"
```

## Producer/Consumer Queue

`ProducerConsumerQueue<T>` and `ProducerConsumerQueueAction<T>` implement the classic
producer/consumer pattern: any thread can act as a *producer*, calling `AddItem`/`AddItems` to
drop work onto the queue, while a single dedicated background thread acts as the *consumer*,
picking items off one at a time (in FIFO order, or sorted order if a `Comparer` is supplied) and
handing each to `ProcessItem`. This serialises processing of the queued items onto one worker
thread without the producers ever blocking on that work.

### IProducerConsumerQueue\<T\> interface

The public contract implemented by `ProducerConsumerQueue<T>`.

| Member | Parameters | Description |
| --- | --- | --- |
| Comparer | `IComparer<T>` (property) | The comparer used to order queued items, or `null` for plain FIFO |
| Clear | | Removes all items currently waiting in the queue |
| AddItem | `T` item | Queues a single item to be processed |
| AddItems | `T[]` items | Queues a batch of items to be processed |

### ProducerConsumerQueue\<T\> abstract class

`where T : class`. Implements `IDisposable` and `IProducerConsumerQueue<T>`. Runs a worker
thread for the lifetime of the instance; override `ProcessItem` (and optionally the other hooks
below) in a derived class to define what happens to each item.

| Property | Type | Description |
| --- | --- | --- |
| AutoCleanupItems | bool | If `true`, each item is `Dispose()`d (when it implements `IDisposable`) after it finishes processing |
| IsTimed | bool | `true` if `Timeout` is less than `TimeSpan.MaxValue` |
| Timeout | `TimeSpan` | How long the consumer thread waits for new work before re-checking; set via the constructor |
| Items | `T[]` | Snapshot of the items currently waiting in the queue |
| ExecutingItems | `T[]` | Snapshot of the items currently being processed |
| ExecutingCount | int | Number of items currently being processed |
| IsExecuting | bool | `true` if `ExecutingCount > 0` |
| Comparer | `IComparer<T>` | The comparer used to order queued items, or `null` for plain FIFO (from `IProducerConsumerQueue<T>`) |

| Constructor | Parameters | Description |
| --- | --- | --- |
| ProducerConsumerQueue() | | Equivalent to `ProducerConsumerQueue(true)` |
| ProducerConsumerQueue(bool) | autoCleanup | Equivalent to `ProducerConsumerQueue(autoCleanup, TimeSpan.MaxValue, null)` |
| ProducerConsumerQueue(bool, TimeSpan, IComparer\<T\>) | autoCleanup, timeout, comparer | Full constructor; starts the worker thread immediately |

| Method Name | Parameters | Description |
| --- | --- | --- |
| Clear | | Removes all items currently waiting in the queue (does not affect an item already being processed) |
| AddItem | `T` item | Queues a single item; throws `ArgumentNullException` if `item` is `null` |
| AddItems | `T[]` items | Queues a batch of items; `null` entries are silently dropped, and a `null`/empty array is a no-op |
| Dispose | | Signals the worker thread to exit, waits for it to finish (`WaitForConsumersToComplete`), then releases the wait handle |

Overridable hooks for derived classes:

| Method Name | Parameters | Description |
| --- | --- | --- |
| WaitForConsumersToComplete | | (protected virtual) Default implementation joins the worker thread; called from `Dispose` |
| CanHandleItemNow | `T` item | (protected virtual) Return `false` to leave the item at the front of the queue and wait rather than process it now; default always returns `true` |
| HandleItem | `T` item | (protected virtual) Calls `ProcessItem` then `ItemComplete`, trapping (and tracing) any exception from either so the worker thread keeps running |
| ItemAdded | `T` item | (protected virtual) Hook called while an item is being enqueued; default does nothing |
| ItemComplete | `T` item | (protected virtual) Hook called after `ProcessItem` completes; default does nothing |
| ProcessItem | `T` item | (protected abstract) Must be implemented to process a dequeued item |

Exceptions thrown out of `HandleItem` itself (not just `ProcessItem`/`ItemComplete`, which are
already individually trapped) are caught in the worker loop and traced, so one bad item never
kills the consumer thread.

### ProducerConsumerQueueAction\<T\> class

A ready-to-use concrete `ProducerConsumerQueue<T>` that delegates `ProcessItem` to an `Action<T>`
supplied at construction time, so you don't need to write a subclass just to process items with a
lambda.

| Constructor | Parameters | Description |
| --- | --- | --- |
| ProducerConsumerQueueAction(Action\<T\>) | action | Uses the default base constructor (`autoCleanup: true`, untimed, no comparer) |
| ProducerConsumerQueueAction(Action\<T\>, bool) | action, autoCleanUp | Passes `autoCleanUp` through to the base constructor |
| ProducerConsumerQueueAction(Action\<T\>, bool, TimeSpan, IComparer\<T\>) | action, autoCleanUp, timeout, comparer | Passes all four through to the base constructor |

Example:

```csharp
public class WorkItem
{
    public int Value { get; set; }
}

var processedValues = new ConcurrentBag<int>();
using var completed = new CountdownEvent(3);

using var queue = new ProducerConsumerQueueAction<WorkItem>(item =>
{
    processedValues.Add(item.Value);
    completed.Signal();
}, autoCleanUp: false);

queue.AddItems([
    new WorkItem { Value = 1 },
    new WorkItem { Value = 2 },
    new WorkItem { Value = 3 }
]);

completed.Wait(TimeSpan.FromSeconds(5));
// processedValues now contains 1, 2 and 3 (order not guaranteed across threads,
// but FIFO relative to each other since no Comparer was supplied)

queue.Items.Length;         // 0 - nothing left waiting
queue.ExecutingCount;       // 0 - the consumer has finished them all
```

## Mutexes

`DNX.Extensions.Threading.Mutexes` provides a *named* in-process lock: unlike
`System.Threading.Monitor`, which locks on an object reference you have to pass around yourself,
a named mutex lets unrelated code anywhere in the process synchronise on a shared string key.

### Mutex class

Implements `IDisposable`. Acquiring a `Mutex` blocks the calling thread (via `Monitor.Enter`)
until the lock object associated with `name` becomes free; disposing it releases the lock.
Lock objects are cached process-wide per name, so two `Mutex` instances created with the same
`name` contend for the same underlying `Monitor`.

| Property | Type | Description |
| --- | --- | --- |
| LockObject | object | The underlying object that `Monitor.Enter`/`Monitor.Exit` is called on |
| State | `MutexState` | The current state of the mutex: `Waiting`, `Acquired` or `Released` |
| Name | string | The name the mutex was constructed with |
| Timestamp | `DateTime` | UTC time the `Mutex` instance was constructed |
| ThreadId | int | `ManagedThreadId` of the thread that constructed (and so acquired) the mutex |

| Constructor | Parameters | Description |
| --- | --- | --- |
| Mutex(string) | name | Blocks until the named lock is acquired, then registers itself with `MutexManager` |

| Method Name | Parameters | Description |
| --- | --- | --- |
| Dispose | | Releases the lock and unregisters from `MutexManager`; safe to call more than once - a second call is a no-op once `State` is `Released` |

### MutexState enum

| Value | Description |
| --- | --- |
| Waiting = 0 | Waiting to acquire a lock |
| Acquired = 1 | Lock acquired |
| Released = 2 | Lock released |

### MutexManager static class

Tracks every currently-acquired named `Mutex` in the process and offers non-blocking ways to
test or attempt acquisition.

| Property | Type | Description |
| --- | --- | --- |
| Mutexes | `IDictionary<string, Mutex>` | All currently-acquired mutexes, keyed by name |

| Static Method | Parameters | Description |
| --- | --- | --- |
| Acquire | string name | Blocks until the named mutex can be acquired, then returns it |
| AcquireNoWait | string name | Returns the acquired `Mutex` immediately if `name` is free, or `null` without blocking if it is already held |
| CanAcquire | string name | Returns `true` if `name` is not currently held (does not acquire it) |

Example:

```csharp
var name = "my-shared-resource";

using (var mutex = MutexManager.Acquire(name))
{
    mutex.State;    // MutexState.Acquired
    MutexManager.CanAcquire(name);       // false - already held
    MutexManager.AcquireNoWait(name);    // null - cannot acquire without waiting
}
// mutex disposed here - lock released

MutexManager.CanAcquire(name);   // true - released again
```

## TaskExtensions

Extension methods on `Task`/`Task<T>` and on `IEnumerable<Task>`/`IEnumerable<Task<T>>` for
timeouts, safe fire-and-forget execution, and awaiting every task in a set while collecting
every failure rather than just the first.

| Method Name | Parameters | Description |
| --- | --- | --- |
| WithTimeout | this `Task` task, `TimeSpan` timeout, `CancellationToken` cancellationToken = default | Awaits `task`; throws `TimeoutException` if it has not completed within `timeout`, or `OperationCanceledException` if `cancellationToken` is cancelled first. The original task is **not** cancelled in either case - it keeps running in the background |
| WithTimeout\<T\> | this `Task<T>` task, `TimeSpan` timeout, `CancellationToken` cancellationToken = default | As above, returning the task's result on success |
| FireAndForget | this `Task` task, `Action<Exception>` exceptionHandler = null | Starts observing `task` without awaiting it, so any exception it throws doesn't go unobserved. A faulted task's exception is passed to `exceptionHandler` if supplied (otherwise silently discarded); a *cancelled* task is always discarded without invoking `exceptionHandler` |
| WhenAllOrThrow | this `IEnumerable<Task>` tasks, `CancellationToken` cancellationToken = default | Awaits every task; if one or more fail, throws an `AggregateException` containing **all** of their exceptions (unlike a plain `await Task.WhenAll(...)`, which surfaces only the first). Throws `OperationCanceledException` immediately if `cancellationToken` is cancelled before the tasks finish. Throws `ArgumentException` if `tasks` is null or empty |
| WhenAllOrThrow\<T\> | this `IEnumerable<Task<T>>` tasks, `CancellationToken` cancellationToken = default | As above, returning each task's result (in the supplied order) on success |

Notes:
- In `WithTimeout`/`WithTimeout<T>`, if `cancellationToken` fires at the same moment the timeout
  would otherwise have elapsed, cancellation wins: `OperationCanceledException` is thrown instead
  of `TimeoutException`.
- `WhenAllOrThrow`/`WhenAllOrThrow<T>` do not cancel the underlying tasks when
  `cancellationToken` is cancelled - like `WithTimeout`, they just stop waiting for them.

Examples:

```csharp
// WithTimeout - completes normally
var quickTask = Task.Delay(TimeSpan.FromMilliseconds(10));
await quickTask.WithTimeout(TimeSpan.FromSeconds(5));
// Returns normally - the delay finished well within the 5 second timeout

// WithTimeout - times out
var neverCompletes = new ManualResetEventSlim();
var slowTask = Task.Run(() => neverCompletes.Wait(TimeSpan.FromSeconds(30)));
await slowTask.WithTimeout(TimeSpan.FromMilliseconds(10));
// Throws TimeoutException - the underlying task keeps running in the background

// WithTimeout<T> - returns the result
var resultTask = Task.FromResult(42);
var value = await resultTask.WithTimeout(TimeSpan.FromSeconds(5));
// value == 42

// FireAndForget - faulted task routed to the handler instead of throwing
Exception caught = null;
var faultingTask = Task.Run(() => throw new InvalidOperationException("boom"));
faultingTask.FireAndForget(ex => caught = ex);
// After the task completes, caught is the InvalidOperationException - nothing is thrown
// on the calling thread, and no unobserved task exception is raised

// WhenAllOrThrow - collects every failure, not just the first
var tasks = new[]
{
    Task.Run(() => throw new InvalidOperationException("first")),
    Task.Run(() => throw new ArgumentException("second")),
};
await tasks.WhenAllOrThrow();
// Throws AggregateException whose InnerExceptions contains both the
// InvalidOperationException and the ArgumentException

// WhenAllOrThrow<T> - results come back in the original order
var typedTasks = new[] { Task.FromResult(1), Task.FromResult(2), Task.FromResult(3) };
var results = await typedTasks.WhenAllOrThrow();
// results == [1, 2, 3]
```
