using Autofac;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;
using System.Reflection;
using WebApiCore.AutoJob;

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


            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<IOCFactory>().As<IJobFactory>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<AutoJobManager>().As<IAutoJobManager>().SingleInstance().PropertiesAutowired();
            //builder.RegisterType<UserJob>().InstancePerLifetimeScope().PropertiesAutowired();


            builder.RegisterAssemblyTypes(Assembly.Load("WebApiCore.AutoJob")).Where(x => x.GetInterfaces().Contains(typeof(IJob)))
                .InstancePerLifetimeScope().PropertiesAutowired();
        }
    }
}
