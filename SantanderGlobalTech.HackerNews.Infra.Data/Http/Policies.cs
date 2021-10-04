using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using Serilog;
using System;
using System.Threading.Tasks;

namespace SantanderGlobalTech.HackerNews.Infra.Data.Http
{
    public static class Policies
    {
        private static readonly ILogger logger = Log.Logger;

        private static readonly AsyncTimeoutPolicy TimeoutPolicy = Policy.TimeoutAsync(3, (context, timeSpan, task) =>
        {
            logger.Warning($"Timeout delegate fired after {timeSpan.Seconds} seconds");
            return Task.CompletedTask;
        });

        private static readonly AsyncRetryPolicy RetryPolicy = Policy.Handle<Exception>()
                                                                     .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) / 2), (exception, timeSpan, retryCount, context) =>
                                                                     {
                                                                         logger.Warning($"Retry delegate fired, attempt {retryCount}");
                                                                     });

        private static readonly AsyncCircuitBreakerPolicy CircuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(2, TimeSpan.FromMinutes(1), (exception, timespan) => { logger.Warning(exception, "Circuit open for {timespan} minutes", timespan.TotalMinutes); }, () => logger.Information("Circuit reset"));


        public static readonly AsyncPolicyWrap PolicyStrategy = Policy.WrapAsync(CircuitBreakerPolicy, RetryPolicy, TimeoutPolicy);
    }
}
