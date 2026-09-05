using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
