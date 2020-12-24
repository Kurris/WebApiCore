using Quartz;
using Quartz.Spi;
using System;
using System.Threading.Tasks;
using WebApiCore.AutoJobInterface;
using WebApiCore.Utils.Extensions;


namespace WebApiCore.AutoJob
{
    public class JobCenter : IJobCenter
    {
        public ISchedulerFactory SchedulerFactory { get; set; }
        public IJobFactory IOCFactory { get; set; }

        public async Task<bool> AddNewJob(int id)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            scheduler.JobFactory = IOCFactory;

            var jobData = await JobHelper.GetAutoJobAsync(id);

            DateTime startTime = jobData.StartTime ?? DateTime.Now;
            DateTime? endTime = jobData.EndTime;

            ITrigger trigger = JobHelper.CreateTrigger(jobData.JobName, jobData.JobGroup, startTime, jobData.CronExpression, endTime);

            IJobDetail jobDetail = JobHelper.CreateDetail<JobExecute>(jobData.JobName, jobData.JobGroup, jobData.DeepCloneByXML());

            await scheduler.ScheduleJob(jobDetail, trigger);
            return true;
        }

        public async Task<bool> EditJob(int id)
        {
            if (await RemoveJob(id))
            {
                return await AddNewJob(id);
            }
            return false;
        }
        public async Task<bool> RemoveJob(int id)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            var jobData = await JobHelper.GetAutoJobAsync(id);
            return await scheduler.DeleteJob(new JobKey(jobData.JobName, jobData.JobGroup));
        }

        public async Task<bool> Start()
        {
            var jobDatas = await JobHelper.GetActiveJobListAsync();

            IScheduler scheduler = await SchedulerFactory.GetScheduler();
            scheduler.JobFactory = IOCFactory;

            try
            {
                foreach (var jobData in jobDatas)
                {
                    DateTime startTime = jobData.StartTime ?? DateTime.Now;
                    DateTime? endTime = jobData.EndTime;

                    ITrigger trigger = JobHelper.CreateTrigger(jobData.JobName, jobData.JobGroup, startTime, jobData.CronExpression, endTime);

                    IJobDetail jobDetail = JobHelper.CreateDetail<JobExecute>(jobData.JobName, jobData.JobGroup, jobData.DeepCloneByXML());

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
