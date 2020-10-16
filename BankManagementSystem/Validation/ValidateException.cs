using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace BankManagementSystem.Validation
{
    public class ValidateException : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "Something went wrong.....!!";
            var exceptionType = context.Exception.GetType();

            if (exceptionType == typeof(NullReferenceException))
            {
                statusCode = HttpStatusCode.NotFound;
                errorMessage = "No Data found";
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new JsonResult(new { error = new[] { errorMessage }, stackTrace = context.Exception.StackTrace });
            base.OnException(context);
        }
    }
}
