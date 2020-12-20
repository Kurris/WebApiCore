using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApiCore.CustomClass
{

    public class CustomResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {
        public ILogger<CustomResourceFilterAttribute> Logger { get; set; }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var nextContext = await next();
        }
    }
}
