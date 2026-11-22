using DNX.Extensions.Services;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Services;

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
            return new Exception("Invalid Id");
        }
        else
        {
            return new User
            {
                UserId = id,
                Name = "Forename Surname"
            };
        }
    }
}

public class ServiceResultTests
{
    internal MyUserService Service = new();

    [Fact]
    public void SuccessResult_can_be_easily_and_successfully_created()
    {
        var userId = Guid.NewGuid();

        // Act
        var result = Service.GetUser(userId);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void FailureResult_can_be_easily_and_successfully_created()
    {
        var userId = Guid.Empty;

        // Act
        var result = Service.GetUser(userId);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Data.ShouldBeNull();
        result.Error.ShouldNotBeNull();
    }

    [Fact]
    public void Success_factory_method_creates_a_successful_result()
    {
        var user = new User { UserId = Guid.NewGuid(), Name = "Bob" };

        var result = ServiceResult<User>.Success(user);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldBe(user);
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void Failure_factory_method_creates_a_failed_result()
    {
        var error = new Exception("Invalid Id");

        var result = ServiceResult<User>.Failure(error);

        result.IsSuccess.ShouldBeFalse();
        result.Data.ShouldBeNull();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void ServiceResult_of_Exception_can_only_be_constructed_via_the_factory_methods()
    {
        // The implicit operators are ambiguous when T is itself Exception, since both
        // "implicit operator ServiceResult<T>(T)" and "implicit operator ServiceResult<T>(Exception)"
        // become the same signature - the explicit factory methods exist for exactly this case.
        var data = new Exception("payload, not a failure");
        var error = new Exception("an actual failure");

        var successResult = ServiceResult<Exception>.Success(data);
        var failureResult = ServiceResult<Exception>.Failure(error);

        successResult.IsSuccess.ShouldBeTrue();
        successResult.Data.ShouldBe(data);

        failureResult.IsSuccess.ShouldBeFalse();
        failureResult.Error.ShouldBe(error);
    }
}
