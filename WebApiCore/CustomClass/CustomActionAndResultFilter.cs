using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Utils;
using WebApiCore.Utils.Model;

namespace Ligy.Project.WebApi.CustomClass
{

    public class CustomActionAndResultFilter : ActionFilterAttribute
    {
        public ILogger<CustomActionAndResultFilter> Logger { get; set; }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"]}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"]}\r\n" +
                       $"【Paras】：{(context.ActionArguments.Count == 0 ? "None" : JsonHelper.ToJson(context.ActionArguments))}";

            Logger.LogInformation(sLog);

            return base.OnActionExecutionAsync(context, next);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"]}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"]}\r\n";

            Logger.LogInformation(sLog);

            if (!context.ModelState.IsValid)
            {
                var result = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key].Errors.Select(x => new EntityErrorParam(key, x.ErrorMessage)));

                context.Result = new ObjectResult(new TData<IEnumerable<EntityErrorParam>>("参数不合法", result, ReturnStatus.ValidateEntityError));
            }
            else
            {
                context.Result = new ObjectResult(context.Result);
            }

            return base.OnResultExecutionAsync(context, next);
        }
    }
}
