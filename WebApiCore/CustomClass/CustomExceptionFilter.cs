using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Ligy.Project.WebApi.CustomClass
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private ILogger<CustomExceptionFilterAttribute> _ilogger = null;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> ilogger)
        {
            _ilogger = ilogger;
        }

        public void OnException(ExceptionContext context)
        {
            var sLog = $"【Source】:{context.Exception.TargetSite}\r\n" +
                        $"【StackTrace】:{context.Exception.StackTrace}\r\n" +
                        $"【ErrorMessage】:{context.Exception.Message}\r\n";
            _ilogger.LogError(sLog);

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(
                new ResultModel(
                    code: 500,
                    message: context.Exception.Message,
                    result: string.Empty,
                    returnStatus: ReturnStatus.Error
                    )
                );
        }
    }
}
