using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApiCore.AutoJobInterface;
using AutoJobEntity = WebApiCore.Entity.SystemManage.AutoJobTask;

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
        public static IJobDetail CreateDetail<T>(string name, string group, AutoJobEntity autoJobData) where T : IJob
        {
            return JobBuilder.Create(typeof(T))
                   .WithIdentity(name, group)
                   .UsingJobData(new JobDataMap(new Dictionary<string, AutoJobEntity>()
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
        public static async Task ExcuteDestineJobAsync(string name, string group, string[] args)
        {
            Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name.Equals("WebApiCore.AutoJob.Job" + "." + name));
            JobPlugin jobPlugin = (JobPlugin)Activator.CreateInstance(type, typeof(string[]));
            jobPlugin.Name = name;
            jobPlugin.Group = group;
            await jobPlugin.Excute(args);
        }


    }
}
