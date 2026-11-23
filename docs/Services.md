# DNX.Extensions.Services

`ServiceResult<T>` is a lightweight readonly struct for returning either a successful value or a
captured exception from a "service" style method, without using exceptions for an anticipated
failure path. Inspired by:
[https://codingbolt.net/2023/10/06/the-serviceresult-pattern/](https://codingbolt.net/2023/10/06/the-serviceresult-pattern/)

## ServiceResult\<T\> struct

| Property | Type | Description |
| --- | --- | --- |
| Data | `T` | The successful payload, or `default(T)` if the result is a failure |
| Error | `Exception` | The captured exception, or `null` if the result is a success |
| IsSuccess | bool | `true` when `Error` is `null` |

Two implicit conversions let a method declared to return `ServiceResult<T>` simply `return` a `T`
or an `Exception` directly, with the struct built for you:

| Member | Parameters | Description |
| --- | --- | --- |
| implicit operator ServiceResult\<T\>(T) | data | Builds a successful result wrapping `data` |
| implicit operator ServiceResult\<T\>(Exception) | error | Builds a failed result wrapping `error` |

When `T` is itself `Exception`, those two implicit operators become identical overloads -
`implicit operator ServiceResult<T>(T)` and `implicit operator ServiceResult<T>(Exception)` both
resolve to the same signature once `T == Exception` - so the compiler can no longer tell which one
you mean and a plain `return someException;` is ambiguous. The static factory methods below exist
specifically to construct a `ServiceResult<Exception>` in that situation (and work equally well
for any other `T`):

| Static Method | Parameters | Description |
| --- | --- | --- |
| Success | `T` data | Creates a successful `ServiceResult<T>` - unambiguous even when `T` is `Exception` |
| Failure | `Exception` error | Creates a failed `ServiceResult<T>` - unambiguous even when `T` is `Exception` |

Example:

```csharp
public class User
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
}

public class MyUserService
{
    public ServiceResult<User> GetUser(Guid id)
    {
        if (id == Guid.Empty)
        {
            return new Exception("Invalid Id");   // implicit operator from Exception
        }

        return new User { UserId = id, Name = "Forename Surname" };  // implicit operator from User
    }
}

var service = new MyUserService();

var result = service.GetUser(Guid.NewGuid());
// result.IsSuccess == true
// result.Data.Name == "Forename Surname"
// result.Error == null

var failure = service.GetUser(Guid.Empty);
// failure.IsSuccess == false
// failure.Data == null
// failure.Error.Message == "Invalid Id"
```

Using the factory methods when `T` is `Exception`:

```csharp
// T is itself Exception - the implicit operators would be ambiguous here, so use the factories
var data = new Exception("payload, not a failure");
var error = new Exception("an actual failure");

var successResult = ServiceResult<Exception>.Success(data);
var failureResult = ServiceResult<Exception>.Failure(error);

// successResult.IsSuccess == true
// successResult.Data == data
// failureResult.IsSuccess == false
// failureResult.Error == error
```
