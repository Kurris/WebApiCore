using Autofac;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;
using System.Reflection;
using WebApiCore.AutoJob;
using WebApiCore.Lib.AutoJob.Abstractions;

namespace WebApiCore.CustomClass
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //业务注入
            builder.RegisterAssemblyTypes(Assembly.Load("WebApiCore.Service"), Assembly.Load("WebApiCore.Interface"))
                   .InstancePerLifetimeScope()
                   .AsImplementedInterfaces()
                   .PropertiesAutowired();

            //控制器和过滤器注入,可使用属性
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsSubclassOf(typeof(ControllerBase))
                         || x.IsSubclassOf(typeof(Attribute))).PropertiesAutowired();

            /*  自动任务注入  */

            //管理类注入
            builder.RegisterType<JobCenter>().As<IJobCenter>().SingleInstance().PropertiesAutowired();
            //单例调度器工厂
            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance().PropertiesAutowired();
            //具体的Job注入
            builder.RegisterAssemblyTypes(Assembly.Load("WebApiCore.AutoJob")).Where(x => x.GetInterfaces().Contains(typeof(IJob)))
               .InstancePerLifetimeScope().PropertiesAutowired();
            //替换调度器的工厂实现,实现Job任务可依赖注入
            builder.RegisterType<IOCFactory>().As<IJobFactory>().SingleInstance().PropertiesAutowired();
        }
    }
}
