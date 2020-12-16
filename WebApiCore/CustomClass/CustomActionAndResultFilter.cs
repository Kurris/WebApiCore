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

    public class CustomActionAndResultFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<CustomActionAndResultFilterAttribute> _logger;

        public ILogger<CustomActionAndResultFilterAttribute>  Logger { get; set; }

        public CustomActionAndResultFilterAttribute(ILogger<CustomActionAndResultFilterAttribute> logger)
        {
            this._logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"]}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"]}\r\n" +
                       $"【Paras】：{(context.ActionArguments.Count == 0 ? "None" : JsonHelper.ToJson(context.ActionArguments))}";

            _logger.LogInformation(sLog);

            return base.OnActionExecutionAsync(context, next);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"]}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"]}\r\n";

            _logger.LogInformation(sLog);

            if (!context.ModelState.IsValid)
            {
                var result = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key].Errors.Select(x => new EntityErrorParam(key, x.ErrorMessage)));

                context.Result = new ObjectResult(new TData<IEnumerable<EntityErrorParam>>("参数不合法", result, Status.ValidateEntityError));
            }
            else
            {
                object resultValue = (context.Result as ObjectResult)?.Value;

                if (resultValue != null && resultValue.GetType().Name.StartsWith("TData"))
                {
                    context.Result = new ObjectResult(resultValue);
                }
                else
                {
                    context.Result = new ObjectResult(new TData<object>("请求成功", resultValue, Status.Success));
                }
            }

            return base.OnResultExecutionAsync(context, next);
        }
    }
}
