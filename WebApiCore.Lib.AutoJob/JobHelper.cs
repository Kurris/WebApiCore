using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Quartz;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity.SystemManage;
using WebApiCore.Lib.AutoJob.Abstractions;
using WebApiCore.Lib.Utils;

namespace WebApiCore.AutoJob
{
    /// <summary>
    /// Job帮助类
    /// </summary>
    public class JobHelper
    {
        /// <summary>
        /// 创建自动任务触发器
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="group">任务组</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="minute">相隔分钟</param>
        /// <param name="sec">相关秒数</param>
        /// <param name="endTime">结束时间</param>
        /// <returns><see cref="ITrigger"/></returns>
        public static ITrigger CreateTrigger(string name, string group, DateTime startTime, int minute, int sec, DateTime? endTime = null)
        {
            bool forever = false;
            if (endTime == null)
            {
                endTime = DateTime.MaxValue;
                forever = true;
            }

            return TriggerBuilder.Create()
                       .WithIdentity(name, group)
                       .StartAt(startTime.AddSeconds(5))
                       .WithSimpleSchedule(x =>
                       {
                           if (minute != 0) x.WithIntervalInMinutes(minute);
                           if (sec != 0) x.WithIntervalInSeconds(sec);
                           if (forever) x.RepeatForever();
                       })
                       .EndAt(endTime)
                       .Build();
        }

        /// <summary>
        /// 创建自动任务触发器
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="group">任务组</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="cronExp">cron表达式</param>
        /// <param name="endTime">结束时间</param>
        /// <returns><see cref="ICronTrigger"/></returns>
        public static ICronTrigger CreateTrigger(string name, string group, DateTime startTime, string cronExp, DateTime? endTime = null)
        {
            if (endTime == null) endTime = DateTime.MaxValue;

            return (ICronTrigger)TriggerBuilder.Create()
                       .WithIdentity(name, group)
                       .StartAt(startTime.AddSeconds(5))
                       .WithCronSchedule(cronExp)
                       .EndAt(endTime)
                       .Build();
        }

        /// <summary>
        /// 创建自动任务细节
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="group">任务组</param>
        /// <param name="autoJobData">任务数据</param>
        /// <returns><see cref="IJobDetail"/></returns>
        public static IJobDetail CreateDetail<T>(string name, string group, AutoJobTask autoJobData) where T : IJob
        {
            return JobBuilder.Create(typeof(T))
                   .WithIdentity(name, group)
                   .UsingJobData(new JobDataMap(new Dictionary<string, AutoJobTask>()
                   {
                       ["data"] = autoJobData
                   })).Build();
        }

        /// <summary>
        /// 执行预定的异步定时任务
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="group">任务组名</param>
        /// <param name="args">任务参数</param>
        /// <returns></returns>
        public static async Task ExecuteDestineJobAsync(string name, string group, string[] args)
        {
            Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name.Equals("WebApiCore.AutoJob.Job" + "." + name));
            if (GlobalInvariant.ServiceProvider.GetService(type) is JobPlugin jobPlugin)
            {
                jobPlugin.Name = name;
                jobPlugin.Group = group;
                await jobPlugin.Excute(args);
            }
        }

        /// <summary>
        /// 执行插件的异步定时任务
        /// </summary>
        /// <param name="executeName">命名空间与类名</param>
        /// <param name="name">任务名称</param>
        /// <param name="group">任务组名</param>
        /// <param name="args">任务参数</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ExecutePluginJobAsync(string executeName, string name, string group, string[] args)
        {
            var arrNamespaceAndClass = executeName.Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (arrNamespaceAndClass.Count() != 2) return;

            string dllName = Path.Combine(Directory.GetCurrentDirectory(), arrNamespaceAndClass.First() + ".dll");
            var loadassembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllName);
            Type type = loadassembly.GetType(arrNamespaceAndClass.First() + "." + arrNamespaceAndClass.Last());
            if (GlobalInvariant.ServiceProvider.GetService(type) is JobPlugin jobPlugin)
            {
                jobPlugin.Name = name;
                jobPlugin.Group = group;
                await jobPlugin.Excute(args);

                AssemblyLoadContext.Default.Unload();
            }
        }


        /// <summary>
        /// 执行存储过程的异步定时任务
        /// </summary>
        /// <param name="executeName">存储过程名称</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ExecuteProcAsync(string executeName)
        {
            await EFDB.Instance.ExecProcAsync(executeName);
        }

        /// <summary>
        /// 执行sql的异步定时任务
        /// </summary>
        /// <param name="executeName">SQL语句</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ExecuteSqlAsync(string executeName)
        {
            await EFDB.Instance.RunSqlAsync(executeName);
        }

        /// <summary>
        /// 获取匹配的任务数据
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns><see cref="Task{AutoJobTask}"/></returns>
        public static async Task<AutoJobTask> GetAutoJobAsync(int id)
        {
            return await EFDB.Create().FindAsync<AutoJobTask>(id);
        }

        /// <summary>
        /// 获取活动任务
        /// </summary>
        /// <returns><see cref="Task{IEnumerable{AutoJobTask}}"/></returns>
        public static async Task<IEnumerable<AutoJobTask>> GetActiveJobListAsync()
        {
            return await EFDB.Create().FindListAsync<AutoJobTask>(x => x.JobStatus == 1);
        }
    }
}
