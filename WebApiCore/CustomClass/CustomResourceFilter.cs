using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Ligy.Project.WebApi.CustomClass
{
    /// <summary>
    /// 缓存作用,在控制器实例化前被调用
    /// </summary>
    public class CustomResourceFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {

        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }


    }
}
