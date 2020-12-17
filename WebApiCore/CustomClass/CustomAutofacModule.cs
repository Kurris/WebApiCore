using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ligy.Project.WebApi.CustomClass
{
    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly iservice = Assembly.Load("WebApiCore.Interface");
            Assembly service = Assembly.Load("WebApiCore.Service");
            builder.RegisterAssemblyTypes(service, iservice)
                   .InstancePerLifetimeScope()
                   .AsImplementedInterfaces()
                   .PropertiesAutowired();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsSubclassOf(typeof(ControllerBase))
                         || x.IsSubclassOf(typeof(Attribute))).PropertiesAutowired();
        }
    }
}
