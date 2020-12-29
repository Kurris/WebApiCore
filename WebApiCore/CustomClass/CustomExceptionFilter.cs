using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApiCore.Lib.Utils.Extensions;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.CustomClass
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ILogger<CustomExceptionFilterAttribute> Logger { get; set; }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            string msg = context.Exception.GetInnerException();

            var sLog = $"【Source】:{context.Exception.TargetSite}\r\n" +
                        $"【StackTrace】:{context.Exception.StackTrace}\r\n" +
                        $"【ErrorMessage】:{msg}\r\n";
            Logger.LogError(sLog);

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(new TData<string>(msg, string.Empty, Status.Error));

            await Task.FromResult(true);
        }
    }
}
