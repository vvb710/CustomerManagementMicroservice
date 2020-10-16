using BankManagementSystem.Logging;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankManagementSystem.Logger
{
    public class LoggingActionFilter : ActionFilterAttribute
    {
        private readonly ILoggerManager _logger;

        public LoggingActionFilter(ILoggerManager logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInfo($"Action '{context.ActionDescriptor.DisplayName}' executing");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInfo($"Action '{context.ActionDescriptor.DisplayName}' executed");
            if (context.Exception != null)
            {
                _logger.LogError($"Exception Occured : '{context.Exception.Message}'");
            }
        }
    }
}
