using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Ligy.Project.WebApi.CustomClass
{

    public class CustomActionAndResultFilter : Attribute, IActionFilter, IResultFilter
    {
        private readonly ILogger<CustomActionAndResultFilter> _logger = null;

        public CustomActionAndResultFilter(ILogger<CustomActionAndResultFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"]}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"]}\r\n" +
                       $"【Paras】：{(context.ActionArguments.Count != 0 ? JsonConvert.SerializeObject(context.ActionArguments) : "None")}";

            _logger.LogInformation(sLog);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"]}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"]}\r\n";

            _logger.LogInformation(sLog);


            if (!context.ModelState.IsValid)
            {
                var result = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)));

                context.Result = new ObjectResult(
                    new ResultModel(
                    code: 422,
                    message: "参数不合法",
                    result: result,
                    returnStatus: ReturnStatus.Fail)
                    );
            }
            else
            {
                context.Result = new ObjectResult(
                                  new ResultModel(
                                  code: 100,
                                  message: "成功",
                                  result: (context.Result as ObjectResult)?.Value,
                                  returnStatus: ReturnStatus.Success)
                                  );
            }
        }
    }
}
