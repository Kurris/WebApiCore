using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using WebApiCore.Utils.Extensions;
using WebApiCore.Interface.SystemManage;


namespace WebApiCore.AutoJob
{
    public class JobCenter : IJobCenter
    {
        public ILogger<JobCenter> Logger { get; set; }
        public ISchedulerFactory SchedulerFactory { get; set; }
        public IJobFactory IOCFactory { get; set; }

        public IAutoJobService AutoJobService { get; set; }

        public async Task<string> Add(string name, string group)
        {
            var scheduler = await SchedulerFactory.GetScheduler();

            var jobData = await AutoJobService.GetAutoJob(name, group);

            DateTime startTime = jobData.StartTime ?? DateTime.Now;
            DateTime? endTime = jobData.EndTime;

            ITrigger trigger;

            if (jobData.Minute.IsEmpty() && jobData.Second.IsEmpty())
                trigger = JobHelper.CreateTrigger(jobData.Name, jobData.Group, startTime, jobData.CronExpression, endTime);
            else
                trigger = JobHelper.CreateTrigger(jobData.Name, jobData.Group, startTime, jobData.Minute.ParseToInt(), jobData.Second.ParseToInt(), endTime);

            IJobDetail jobDetail = JobHelper.CreateDetail<JobExcute>(jobData.Name, jobData.Group, jobData.DeepCloneByXML());

            await scheduler.ScheduleJob(jobDetail, trigger);

            return $"添加任务{name}成功";
        }

        public async Task<string> Edit(string name, string group)
        {
            await Remove(name, group);
            await Add(name, group);

            return $"编辑任务{name}成功";
        }

        public async Task<string> Remove(string name, string group)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            bool deleteResult = await scheduler.DeleteJob(new JobKey(name, group));
            if (deleteResult)
            {
                return await Task.FromResult($"任务{name}已被移除");
            }

            return await Task.FromResult($"任务{name}移除失败");
        }


        public async Task<string> Start()
        {
            var jobDatas = await AutoJobService.GetActiveJobList();

            try
            {
                foreach (var jobData in jobDatas)
                {
                    IScheduler scheduler = await SchedulerFactory.GetScheduler();
                    scheduler.JobFactory = IOCFactory;
                    await scheduler.Start();
                    DateTime startTime = jobData.StartTime ?? DateTime.Now;
                    DateTime? endTime = jobData.EndTime;

                    ITrigger trigger;

                    //if (jobData.Minute == 0 && jobData.Second == 0)
                        trigger = JobHelper.CreateTrigger(jobData.Name, jobData.Group, startTime, jobData.CronExpression, endTime);
                    //else
                    //    trigger = JobHelper.CreateTrigger(jobData.Name, jobData.Group, startTime, jobData.Minute.ParseToInt(), jobData.Second.ParseToInt(), endTime);

                    IJobDetail jobDetail = JobHelper.CreateDetail<JobExcute>(jobData.Name, jobData.Group, jobData.DeepCloneByXML());

                    await scheduler.ScheduleJob(jobDetail, trigger);

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"自动任务开始终止");
                return await Task.FromResult($"自动任务开始终止:{ex.Message}");
            }

            return await Task.FromResult("自动任务开启完成");
        }


        public async Task<string> StopAll()
        {
            var scheduer = await SchedulerFactory.GetScheduler();
            await scheduer.PauseAll();
            return await Task.FromResult("自动任务已全部停止");
        }
    }
}
