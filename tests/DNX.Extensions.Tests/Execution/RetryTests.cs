using System.Diagnostics;
using DNX.Extensions.Execution;
using Shouldly;
using Xunit;

// ReSharper disable InconsistentNaming
// ReSharper disable AccessToModifiedClosure

namespace DNX.Extensions.Tests.Execution;

public class RetryTests
{
    public class DefaultDelayProvider_Tests
    {
        [Theory]
        [InlineData(1, 100)]
        [InlineData(2, 200)]
        [InlineData(3, 400)]
        [InlineData(4, 800)]
        [InlineData(5, 1600)]
        public void Doubles_the_delay_for_each_successive_attempt(int attempt, int expectedMilliseconds)
        {
            var delay = Retry.DefaultDelayProvider(attempt);

            delay.ShouldBe(TimeSpan.FromMilliseconds(expectedMilliseconds));
        }

        [Fact]
        public void Is_deterministic_for_the_same_attempt_number()
        {
            Retry.DefaultDelayProvider(3).ShouldBe(Retry.DefaultDelayProvider(3));
        }

        [Fact]
        public void Matches_MillisecondExponentialBackoff_of_100ms()
        {
            var defaultProvider = Retry.DelayProviders.MillisecondExponentialBackoff(100);

            for (var attempt = 1; attempt <= 5; ++attempt)
            {
                Retry.DefaultDelayProvider(attempt).ShouldBe(defaultProvider(attempt));
            }
        }
    }

    public class DelayProviders_Tests
    {
        [Theory]
        [InlineData(1, 100)]
        [InlineData(2, 200)]
        [InlineData(3, 400)]
        [InlineData(4, 800)]
        public void MillisecondExponentialBackoff_doubles_the_delay_for_each_successive_attempt(int attempt, int expectedMilliseconds)
        {
            var delayProvider = Retry.DelayProviders.MillisecondExponentialBackoff(100);

            delayProvider(attempt).ShouldBe(TimeSpan.FromMilliseconds(expectedMilliseconds));
        }

        [Fact]
        public void MillisecondExponentialBackoff_scales_from_the_supplied_base_delay()
        {
            var delayProvider = Retry.DelayProviders.MillisecondExponentialBackoff(50);

            delayProvider(1).ShouldBe(TimeSpan.FromMilliseconds(50));
            delayProvider(2).ShouldBe(TimeSpan.FromMilliseconds(100));
            delayProvider(3).ShouldBe(TimeSpan.FromMilliseconds(200));
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(2, 200)]
        [InlineData(3, 300)]
        [InlineData(4, 400)]
        public void MillisecondLinearBackoff_grows_the_delay_by_a_constant_step(int attempt, int expectedMilliseconds)
        {
            var delayProvider = Retry.DelayProviders.MillisecondLinearBackoff(100);

            delayProvider(attempt).ShouldBe(TimeSpan.FromMilliseconds(expectedMilliseconds));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 4)]
        [InlineData(4, 8)]
        public void SecondsExponentialBackoff_doubles_the_delay_for_each_successive_attempt(int attempt, int expectedSeconds)
        {
            var delayProvider = Retry.DelayProviders.SecondsExponentialBackoff(1);

            delayProvider(attempt).ShouldBe(TimeSpan.FromSeconds(expectedSeconds));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 4)]
        [InlineData(3, 6)]
        [InlineData(4, 8)]
        public void SecondsLinearBackoff_grows_the_delay_by_a_constant_step(int attempt, int expectedSeconds)
        {
            var delayProvider = Retry.DelayProviders.SecondsLinearBackoff(2);

            delayProvider(attempt).ShouldBe(TimeSpan.FromSeconds(expectedSeconds));
        }

        [Fact]
        public void Each_factory_returns_a_deterministic_provider_for_the_same_attempt_number()
        {
            var exponential = Retry.DelayProviders.MillisecondExponentialBackoff(100);
            var linear = Retry.DelayProviders.MillisecondLinearBackoff(100);

            exponential(3).ShouldBe(exponential(3));
            linear(3).ShouldBe(linear(3));
        }

        [Fact]
        public async Task A_DelayProviders_factory_can_be_used_directly_as_Retrys_delayProvider()
        {
            var attempts = 0;

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
                delayProvider: Retry.DelayProviders.MillisecondLinearBackoff(1),
                cancellationToken: TestContext.Current.CancellationToken);

            attempts.ShouldBe(2);
        }
    }

    public class Execute_Tests
    {
        [Fact]
        public void Uses_the_default_delay_provider_when_none_is_supplied()
        {
            var attempts = 0;
            var stopwatch = Stopwatch.StartNew();

            Retry.Execute(
                () =>
                {
                    ++attempts;
                    if (attempts < 2)
                    {
                        throw new InvalidOperationException("not yet");
                    }
                },
                cancellationToken: TestContext.Current.CancellationToken);

            stopwatch.Stop();

            // DefaultDelayProvider(1) is 100ms - allow generous headroom for scheduler jitter
            // while still proving a real delay (not TimeSpan.Zero) was actually awaited.
            stopwatch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(80));
        }

        [Fact]
        public void Succeeds_without_retrying_when_the_first_attempt_succeeds()
        {
            var attempts = 0;

            Retry.Execute(() => { ++attempts; }, delayProvider: _ => TimeSpan.Zero, cancellationToken: TestContext.Current.CancellationToken);

            attempts.ShouldBe(1);
        }

        [Fact]
        public void Retries_until_it_succeeds()
        {
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
                delayProvider: _ => TimeSpan.Zero,
                cancellationToken: TestContext.Current.CancellationToken);

            attempts.ShouldBe(3);
        }

        [Fact]
        public void Throws_the_final_exception_once_every_attempt_has_failed()
        {
            var attempts = 0;

            var ex = Should.Throw<InvalidOperationException>(() =>
                Retry.Execute(
                    () =>
                    {
                        ++attempts;
                        throw new InvalidOperationException($"attempt {attempts}");
                    },
                    maxAttempts: 3,
                    delayProvider: _ => TimeSpan.Zero));

            attempts.ShouldBe(3);
            ex.Message.ShouldBe("attempt 3");
        }

        [Fact]
        public void Invokes_onRetry_for_every_failed_attempt_except_the_last()
        {
            var attempts = 0;
            var retryNotifications = new List<(Exception Exception, int Attempt)>();

            Should.Throw<InvalidOperationException>(() =>
                Retry.Execute(
                    () =>
                    {
                        ++attempts;
                        throw new InvalidOperationException($"attempt {attempts}");
                    },
                    maxAttempts: 3,
                    delayProvider: _ => TimeSpan.Zero,
                    onRetry: (ex, attempt) => retryNotifications.Add((ex, attempt))));

            retryNotifications.Count.ShouldBe(2);
            retryNotifications[0].Attempt.ShouldBe(1);
            retryNotifications[1].Attempt.ShouldBe(2);
        }

        [Fact]
        public void Does_not_retry_when_shouldRetry_returns_false()
        {
            var attempts = 0;

            Should.Throw<ArgumentException>(() =>
                Retry.Execute(
                    () =>
                    {
                        ++attempts;
                        throw new ArgumentException("not retryable");
                    },
                    maxAttempts: 5,
                    delayProvider: _ => TimeSpan.Zero,
                    shouldRetry: ex => ex is not ArgumentException));

            attempts.ShouldBe(1);
        }

        [Fact]
        public void Does_not_retry_cancellation()
        {
            var attempts = 0;

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Should.Throw<OperationCanceledException>(() =>
                Retry.Execute(
                    () =>
                    {
                        ++attempts;
                        throw new OperationCanceledException(cts.Token);
                    },
                    maxAttempts: 5,
                    delayProvider: _ => TimeSpan.Zero));

            attempts.ShouldBe(1);
        }

        [Fact]
        public void Stops_waiting_between_attempts_when_cancelled()
        {
            using var cts = new CancellationTokenSource();

            Should.Throw<OperationCanceledException>(() =>
                Retry.Execute(
                    () =>
                    {
                        cts.Cancel();
                        throw new InvalidOperationException("fails every time");
                    },
                    maxAttempts: 5,
                    delayProvider: _ => TimeSpan.FromSeconds(30),
                    cancellationToken: cts.Token));
        }
    }

    public class ExecuteT_Tests
    {
        [Fact]
        public void Returns_the_result_once_it_succeeds()
        {
            var attempts = 0;

            var result = Retry.Execute(
                () =>
                {
                    ++attempts;
                    if (attempts < 2)
                    {
                        throw new InvalidOperationException("not yet");
                    }

                    return 42;
                },
                delayProvider: _ => TimeSpan.Zero,
                cancellationToken: TestContext.Current.CancellationToken);

            result.ShouldBe(42);
            attempts.ShouldBe(2);
        }
    }

    public class ExecuteAsync_Tests
    {
        [Fact]
        public async Task Retries_until_it_succeeds()
        {
            var attempts = 0;

            await Retry.ExecuteAsync(
                async () =>
                {
                    ++attempts;
                    if (attempts < 3)
                    {
                        throw new InvalidOperationException("not yet");
                    }

                    await Task.CompletedTask;
                },
                maxAttempts: 5,
                delayProvider: _ => TimeSpan.Zero,
                cancellationToken: TestContext.Current.CancellationToken);

            attempts.ShouldBe(3);
        }

        [Fact]
        public async Task Throws_the_final_exception_once_every_attempt_has_failed()
        {
            var attempts = 0;

            var ex = await Should.ThrowAsync<InvalidOperationException>(() =>
                Retry.ExecuteAsync(
                    () =>
                    {
                        ++attempts;
                        throw new InvalidOperationException($"attempt {attempts}");
                    },
                    maxAttempts: 3,
                    delayProvider: _ => TimeSpan.Zero));

            attempts.ShouldBe(3);
            ex.Message.ShouldBe("attempt 3");
        }
    }

    public class ExecuteAsyncT_Tests
    {
        [Fact]
        public async Task Returns_the_result_once_it_succeeds()
        {
            var attempts = 0;

            var result = await Retry.ExecuteAsync(
                async () =>
                {
                    ++attempts;
                    if (attempts < 2)
                    {
                        throw new InvalidOperationException("not yet");
                    }

                    await Task.CompletedTask;

                    return 42;
                },
                delayProvider: _ => TimeSpan.Zero,
                cancellationToken: TestContext.Current.CancellationToken);

            result.ShouldBe(42);
            attempts.ShouldBe(2);
        }

        [Fact]
        public async Task Does_not_retry_when_shouldRetry_returns_false()
        {
            var attempts = 0;

            await Should.ThrowAsync<ArgumentException>(() =>
                Retry.ExecuteAsync<int>(
                    () =>
                    {
                        ++attempts;
                        throw new ArgumentException("not retryable");
                    },
                    maxAttempts: 5,
                    delayProvider: _ => TimeSpan.Zero,
                    shouldRetry: ex => ex is not ArgumentException));

            attempts.ShouldBe(1);
        }
    }
}
