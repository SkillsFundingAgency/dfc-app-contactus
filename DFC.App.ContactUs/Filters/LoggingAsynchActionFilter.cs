using DFC.Logger.AppInsights.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Filters
{
    public class LoggingAsynchActionFilter : IAsyncActionFilter
    {
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly ILogger<LoggingAsynchActionFilter> logger;

        public LoggingAsynchActionFilter(ICorrelationIdProvider correlationIdProvider, ILogger<LoggingAsynchActionFilter> logger)
        {
            this.correlationIdProvider = correlationIdProvider;
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var correlationId = correlationIdProvider.CorrelationId;

            logger.LogInformation($"CorrelationId:{correlationId} Executing {context.ActionDescriptor.DisplayName}");

            var executed = await next().ConfigureAwait(false);

            if (executed.Exception != null)
            {
                logger.LogError(executed.Exception, $"CorrelationId:{correlationId} Executed {context.ActionDescriptor.DisplayName} with exception {executed.Exception}");
            }
            else
            {
                logger.LogInformation($"CorrelationId:{correlationId} Completed {context.ActionDescriptor.DisplayName}");
            }
        }
    }
}