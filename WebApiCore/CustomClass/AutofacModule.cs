using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WebApiCore.AutoJob;
using WebApiCore.Lib.AutoJob.Abstractions;
using WebApiCore.Lib.Utils;

namespace WebApiCore.CustomClass
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string local = AppContext.BaseDirectory;

            #region 缓存注入

            var cacheImp = Assembly.LoadFrom(Path.Combine(local, "WebApiCore.Lib.Cache.dll")).GetTypes().Where(x=>x.Name.StartsWith(GlobalInvariant.SystemConfig.CacheProvider)).FirstOrDefault();
            builder.RegisterType(cacheImp).InstancePerLifetimeScope().AsImplementedInterfaces().PropertiesAutowired();

            #endregion

            #region 业务注入
            var businessService = Assembly.LoadFrom(Path.Combine(local, "WebApiCore.Business.Service.dll"));
            var businessAbstraction = Assembly.LoadFrom(Path.Combine(local, "WebApiCore.Business.Abstractions.dll"));
            builder.RegisterAssemblyTypes(businessService, businessAbstraction).InstancePerLifetimeScope()
                                                                               .AsImplementedInterfaces()
                                                                               .PropertiesAutowired();

            #endregion

            #region 控制器和过滤器
            //控制器和过滤器注入,可使用属性
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsSubclassOf(typeof(ControllerBase))
                         || x.IsSubclassOf(typeof(Attribute))).PropertiesAutowired();
            #endregion

            #region 自动任务注入

            //管理类注入
            builder.RegisterType<JobCenter>().As<IJobCenter>().SingleInstance().PropertiesAutowired();
            //单例调度器工厂
            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance().PropertiesAutowired();
            //具体的Job注入
            builder.RegisterAssemblyTypes(Assembly.Load("WebApiCore.Lib.AutoJob")).Where(x => x.GetInterfaces().Contains(typeof(IJob)))
               .InstancePerLifetimeScope().PropertiesAutowired();
            //替换调度器的工厂实现,实现Job任务可依赖注入
            builder.RegisterType<IOCFactory>().As<IJobFactory>().SingleInstance().PropertiesAutowired(); 
            #endregion
        }
    }
}
