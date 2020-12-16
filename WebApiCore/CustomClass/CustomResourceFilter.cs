using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Ligy.Project.WebApi.CustomClass
{

    public class CustomResourceFilterAttribute : Attribute, IResourceFilter
    {
        private readonly ILogger<CustomResourceFilterAttribute> _logger;

        public CustomResourceFilterAttribute(ILogger<CustomResourceFilterAttribute> logger)
        {
            this._logger = logger;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {

        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }


    }
}
