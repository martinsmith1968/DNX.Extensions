using DNX.Extensions.Execution;
using Shouldly;
using Xunit;

// ReSharper disable PreferConcreteValueOverDefault
// ReSharper disable InconsistentNaming

namespace DNX.Extensions.Tests.Execution;

public class RunSafelyTests
{
    public class Execute_Tests
    {
        [Fact]
        public void Can_run_simple_action_that_succeeds()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var value = Guid.Empty;

            // Act
            RunSafely.Execute(() => value = guid);

            // Assert
            value.ShouldBe(guid);
        }

        [Fact]
        public void Can_handle_simple_action_that_fails()
        {
            // Act
            RunSafely.Execute(() => throw new Exception(nameof(Can_handle_simple_action_that_fails)));
        }

        [Fact]
        public void Can_handle_simple_action_that_fails_and_extract_exception()
        {
            // Arrange
            var value = int.MaxValue;
            var guid = Guid.NewGuid();
            var message = "";

            // Act
            RunSafely.Execute(() => throw new Exception(guid.ToString()), ex => message = ex.Message);

            // Assert
            value.ShouldBe(int.MaxValue);
            message.ShouldNotBeNull();
            message.ShouldNotBeNull(guid.ToString());
        }

        [Fact]
        public void Can_handle_simple_func_that_fails()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var value = 0;

            // Act
            RunSafely.Execute(() => value = dividend / divisor);

            // Assert
            value.ShouldBe(0);
        }

        [Fact]
        public void Can_handle_simple_func_that_fails_and_extract_exception()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var value = 0;
            var message = "";

            // Act
            RunSafely.Execute(() => value = dividend / divisor, ex => message = ex.Message);

            // Assert
            value.ShouldBe(0);
            message.ShouldNotBeNullOrEmpty();
            message.ShouldContain("divide by zero");
        }
    }

    public class ExecuteT_Tests
    {
        [Fact]
        public void Can_run_simple_func_that_succeeds()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var value = RunSafely.Execute(() => guid);

            // Assert
            value.ShouldBe(guid);
        }

        [Fact]
        public void Can_handle_simple_func_that_fails()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;

            // Act
            var value = RunSafely.Execute(() => dividend / divisor);

            // Assert
            value.ShouldBe(default);
        }

        [Fact]
        public void Can_handle_simple_func_with_default_that_fails()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var defaultResult = 500;

            // Act
            var value = RunSafely.Execute(() => dividend / divisor, defaultResult);

            // Assert
            value.ShouldBe(defaultResult);
        }

        [Fact]
        public void Can_handle_simple_func_that_fails_and_extract_exception()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var message = "";

            // Act
            var value = RunSafely.Execute(() => dividend / divisor, ex => message = ex.Message);

            // Assert
            value.ShouldBe(default);
            message.ShouldNotBeNullOrEmpty();
            message.ShouldContain("divide by zero");
        }

        [Fact]
        public void Can_handle_simple_func_with_default_that_fails_and_extract_exception()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var defaultResult = 500;
            var message = "";

            // Act
            var value = RunSafely.Execute(() => dividend / divisor, defaultResult, ex => message = ex.Message);

            // Assert
            value.ShouldBe(defaultResult);
            message.ShouldNotBeNullOrEmpty();
            message.ShouldContain("divide by zero");
        }
    }

    public class ExecuteAsync_Tests
    {
        [Fact]
        public async Task Can_run_simple_task_that_succeeds()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 20;
            var value = 0;

            var task = new Task(() => value = dividend / divisor);
            task.Start();

            // Act
            await RunSafely.ExecuteAsync(task);

            // Assert
            value.ShouldBe(dividend / divisor);
        }

        [Fact]
        public async Task Can_handle_simple_action_that_fails()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var value = 0;

            var task = new Task(() => value = dividend / divisor);
            task.Start();

            // Act
            await RunSafely.ExecuteAsync(task);

            // Assert
            value.ShouldBe(0);
        }

        [Fact]
        public async Task Can_handle_simple_action_that_fails_and_extract_exception()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var value = 0;
            var message = "";

            var task = new Task(() => value = dividend / divisor);
            task.Start();

            // Act
            await RunSafely.ExecuteAsync(task, ex => message = ex.Message);

            // Assert
            value.ShouldBe(0);
            message.ShouldNotBeNullOrEmpty();
            message.ShouldContain("divide by zero");
        }
    }

    public class ExecuteAsyncT_Tests
    {
        private static async Task<int> DivideAsync(int dividend, int divisor)
        {
            var quotient = await Task.Run(() => dividend / divisor);

            return quotient;
        }

        [Fact]
        public async Task Can_run_simple_func_that_succeeds()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 50;

            // Act
            var value = await RunSafely.ExecuteAsync(DivideAsync(dividend, divisor));

            // Assert
            value.ShouldBe(dividend / divisor);
        }

        [Fact]
        public async Task Can_run_simple_func_that_fails()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var value = 0;

            // Act
            value = await RunSafely.ExecuteAsync(DivideAsync(dividend, divisor));

            // Assert
            value.ShouldBe(default);
        }

        [Fact]
        public async Task Can_run_simple_func_with_default_that_fails()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var defaultResult = 500;
            var value = 0;

            // Act
            value = await RunSafely.ExecuteAsync(DivideAsync(dividend, divisor), defaultResult);

            // Assert
            value.ShouldBe(defaultResult);
        }

        [Fact]
        public async Task Can_handle_simple_func_that_fails_and_extract_exception()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var value = 0;
            var message = "";

            // Act
            value = await RunSafely.ExecuteAsync(DivideAsync(dividend, divisor), ex => message = ex.Message);

            // Assert
            value.ShouldBe(default);
            message.ShouldNotBeNull();
            message.ShouldNotBeNull("divide by zero");
        }

        [Fact]
        public async Task Can_handle_simple_func_with_default_that_fails_and_extract_exception()
        {
            // Arrange
            var dividend = 1000;
            var divisor = 0;
            var defaultResult = 500;
            var value = 0;
            var message = "";

            // Act
            value = await RunSafely.ExecuteAsync(DivideAsync(dividend, divisor), defaultResult,
                ex => message = ex.Message);

            // Assert
            value.ShouldBe(defaultResult);
            message.ShouldNotBeNull();
            message.ShouldContain("divide by zero");
        }
    }
}
