# DNX.Extensions.Collections

Extension methods for the `HashSet<T>`, `Queue<T>` and `Stack<T>` collection types. This
namespace is new to DNX.Extensions - there is no equivalent in the predecessor DNX.Helpers
library.

## HashSetExtensions

Extend `HashSet<T>`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsNullOrEmpty\<T\> | | `true` if the set is null or has zero elements; otherwise `false` |
| AddRange\<T\> | `IEnumerable<T>` items | Adds each item to the set, skipping any that are already present. Returns the count of items that were actually added. Throws `ArgumentNullException` if the set is null; does nothing (and returns 0) if `items` is null |
| ToHashSet\<T\> | `IEnumerable<T>` source (extension target), comparer (optional `IEqualityComparer<T>`) | Creates a `HashSet<T>` from the source sequence, using the supplied comparer if given. **Only compiled for the netstandard2.0 target** (guarded by `#if NETSTANDARD2_0`), because net8.0/net10.0 already ship `Enumerable.ToHashSet` in the BCL - defining a duplicate there would cause an ambiguous-call compile error. If you don't see it in IntelliSense on a newer target, that's why: the framework's own version is what you're calling |

Examples:

```csharp
// AddRange
var set = new HashSet<int> { 1, 2 };
var addedCount = set.AddRange([2, 3, 4]);
// addedCount = 2
// set = { 1, 2, 3, 4 }

// ToHashSet (netstandard2.0 only - on net8.0/net10.0 this resolves to Enumerable.ToHashSet instead)
IEnumerable<string> source = ["a", "A", "b"];
var result = source.ToHashSet(StringComparer.OrdinalIgnoreCase);
// result.Count = 2
```

## QueueExtensions

Extend `Queue<T>`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsNullOrEmpty\<T\> | | `true` if the queue is null or has zero elements; otherwise `false` |
| EnqueueRange\<T\> | `IEnumerable<T>` items | Enqueues each item, in the order supplied. Throws `ArgumentNullException` if the queue is null; does nothing if `items` is null |

Example:

```csharp
var queue = new Queue<int>([1, 2]);

queue.EnqueueRange([3, 4]);

queue.ToArray(); // [1, 2, 3, 4]
```

## StackExtensions

Extend `Stack<T>`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsNullOrEmpty\<T\> | | `true` if the stack is null or has zero elements; otherwise `false` |
| PushRange\<T\> | `IEnumerable<T>` items | Pushes each item onto the stack, in the order supplied - so the last item in the sequence ends up on top. Throws `ArgumentNullException` if the stack is null; does nothing if `items` is null |

Example:

```csharp
var stack = new Stack<int>();

stack.PushRange([1, 2, 3]);

stack.Pop(); // 3
stack.Pop(); // 2
stack.Pop(); // 1
```
