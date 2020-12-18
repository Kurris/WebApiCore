using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace WebApiCore.CustomClass
{

    public class CustomResourceFilterAttribute : Attribute, IResourceFilter
    {
        public ILogger<CustomResourceFilterAttribute>  Logger { get; set; }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {

        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }


    }
}
