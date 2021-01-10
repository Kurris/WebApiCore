using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApiCore.Lib.Model;

namespace WebApiCore.CustomClass
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ILogger<CustomExceptionFilterAttribute> Logger { get; set; }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            Exception ex = context.Exception.GetBaseException();

            var sLog = $"【Source】:{ex.TargetSite}\r\n" +
                        $"【StackTrace】:{ex.StackTrace}\r\n" +
                        $"【ErrorMessage】:{ex.Message}\r\n";
            Logger.LogError(sLog);

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(new TData<string>(ex.Message, string.Empty, Status.Error));

            await Task.FromResult(true);
        }
    }
}
