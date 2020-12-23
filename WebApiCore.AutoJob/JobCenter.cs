using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading.Tasks;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Extensions;


namespace WebApiCore.AutoJob
{
    public class JobCenter : IJobCenter
    {
        public ILogger<JobCenter> Logger { get; set; }
        public ISchedulerFactory SchedulerFactory { get; set; }
        public IJobFactory IOCFactory { get; set; }

        public IAutoJobService AutoJobService { get; set; }

        public async Task<bool> AddNewJob(string name, string group)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            scheduler.JobFactory = IOCFactory;

            var jobData = await AutoJobService.GetAutoJob(name, group);

            DateTime startTime = jobData.StartTime ?? DateTime.Now;
            DateTime? endTime = jobData.EndTime;

            ITrigger trigger = JobHelper.CreateTrigger(jobData.JobName, jobData.JobGroup, startTime, jobData.CronExpression, endTime);

            IJobDetail jobDetail = JobHelper.CreateDetail<JobExcute>(jobData.JobName, jobData.JobGroup, jobData.DeepCloneByXML());

            await scheduler.ScheduleJob(jobDetail, trigger);
            return true;
        }

        public async Task<bool> EditJob(string name, string group)
        {
            if (await RemoveJob(name, group))
            {
                return await AddNewJob(name, group);
            }
            return false;
        }
        public async Task<bool> RemoveJob(string name, string group)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            return await scheduler.DeleteJob(new JobKey(name, group));
        }

        public async Task<bool> Start()
        {
            var jobDatas = await AutoJobService.GetActiveJobList();

            IScheduler scheduler = await SchedulerFactory.GetScheduler();
            scheduler.JobFactory = IOCFactory;

            try
            {
                foreach (var jobData in jobDatas)
                {
                    DateTime startTime = jobData.StartTime ?? DateTime.Now;
                    DateTime? endTime = jobData.EndTime;

                    ITrigger trigger = JobHelper.CreateTrigger(jobData.JobName, jobData.JobGroup, startTime, jobData.CronExpression, endTime);

                    IJobDetail jobDetail = JobHelper.CreateDetail<JobExcute>(jobData.JobName, jobData.JobGroup, jobData.DeepCloneByXML());

                    await scheduler.ScheduleJob(jobDetail, trigger);
                }
                await scheduler.Start();
                return true;
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> StopAll()
        {
            var scheduer = await SchedulerFactory.GetScheduler();
            await scheduer.PauseAll();
            return true;
        }
    }
}
