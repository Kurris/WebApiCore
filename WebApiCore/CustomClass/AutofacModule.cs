using Autofac;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;

namespace WebApiCore.CustomClass
{
    public class AutofacModule : Autofac.Module
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
