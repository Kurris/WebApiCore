using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ligy.Project.WebApi.CustomClass
{
    public class CustomActionFilterAttribute : Attribute, IActionFilter, IResultFilter
    {
        private ILogger<CustomActionFilterAttribute> _logger = null;

        public CustomActionFilterAttribute(ILogger<CustomActionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public  void OnActionExecuting(ActionExecutingContext context)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"].ToString()}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"].ToString()}\r\n" +
                       $"【Paras】：{( context.ActionArguments.Count != 0 ? JsonConvert.SerializeObject(context.ActionArguments) : "None" )}";

            _logger.LogInformation(sLog);
        }
        public  void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public  void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public  void OnResultExecuting(ResultExecutingContext context)
        {
            var sLog = $"【Controller】:{context.RouteData.Values["controller"].ToString()}\r\n" +
                       $"【Action】:{context.RouteData.Values["action"].ToString()}\r\n";

            _logger.LogInformation(sLog);


            if( !context.ModelState.IsValid )
            {
                var result = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key , x.ErrorMessage)));


                context.Result = new ObjectResult(
                    new ResultModel(
                    code: 422 ,
                    message: "参数不合法" ,
                    result: result ,
                    returnStatus: ReturnStatus.Fail)
                    );
            }
        }
    }
}
