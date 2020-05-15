using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

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
