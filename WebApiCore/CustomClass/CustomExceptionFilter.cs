using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiCore.Utils.Extensions;
using WebApiCore.Utils.Model;

namespace Ligy.Project.WebApi.CustomClass
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;
        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            this._logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            string msg = context.Exception.GetInnerException();

            var sLog = $"【Source】:{context.Exception.TargetSite}\r\n" +
                        $"【StackTrace】:{context.Exception.StackTrace}\r\n" +
                        $"【ErrorMessage】:{msg}\r\n";
            _logger.LogError(sLog);

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(new TData<string>(msg, string.Empty, Status.Error));

            return Task.CompletedTask;
        }
    }
}
