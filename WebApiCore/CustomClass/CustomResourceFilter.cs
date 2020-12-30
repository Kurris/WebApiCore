using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApiCore.Lib.CacheAbstractions;

namespace WebApiCore.CustomClass
{

    public class CustomResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {
        public ILogger<CustomResourceFilterAttribute> Logger { get; set; }
        public ICache Cache { get; set; }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            ResourceExecutedContext executedContext = null;

            var path = context.HttpContext.Request.Path;
            if (path.StartsWithSegments("/api/Blog/GetBlogs"))
            {
                var result = Cache.GetCache<ObjectResult>(path);
                if (result != null)
                {
                    context.Result = result;
                }
                else
                {
                    executedContext = await next();                
                }
            }
            else
            {
                executedContext = await next();
            }

            if (executedContext != null)
            {
                if (path.StartsWithSegments("/api/Blog/GetBlogs"))
                {
                    Cache.SetCache(path, (ObjectResult)executedContext.Result);
                }
            }
        }
    }
}
