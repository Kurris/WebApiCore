using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;
using WebApiCore.Lib.AutoJob.Abstractions;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.AutoJob
{
    /// <summary>
    /// 任务中心
    /// </summary>
    public class JobCenter : IJobCenter
    {
        public ISchedulerFactory SchedulerFactory { get; set; }
        public IJobFactory IOCFactory { get; set; }


        #region 开始任务

        public async Task<bool> Start(int id)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            scheduler.JobFactory = IOCFactory;

            var jobData = await JobHelper.GetAutoJobAsync(id);
            if (jobData.JobStatus == 0)
            {
                return false;
            }
            DateTime startTime = jobData.StartTime ?? DateTime.Now;
            DateTime? endTime = jobData.EndTime;

            ITrigger trigger = JobHelper.CreateTrigger(jobData.JobName, jobData.JobGroup, startTime, jobData.CronExpression, endTime);
            IJobDetail jobDetail = JobHelper.CreateDetail<JobExecute>(jobData.JobName, jobData.JobGroup, jobData.DeepCloneByXML());

            await scheduler.ScheduleJob(jobDetail, trigger);
            return true;
        }

        #endregion

        #region 开始所有任务

        public async Task<bool> Start()
        {
            var jobDatas = await JobHelper.GetActiveJobListAsync();

            IScheduler scheduler = await SchedulerFactory.GetScheduler();
            scheduler.JobFactory = IOCFactory;


            foreach (var jobData in jobDatas)
            {
                try
                {
                    DateTime startTime = jobData.StartTime ?? DateTime.Now;
                    DateTime? endTime = jobData.EndTime;

                    ITrigger trigger = JobHelper.CreateTrigger(jobData.JobName, jobData.JobGroup, startTime, jobData.CronExpression, endTime);
                    IJobDetail jobDetail = JobHelper.CreateDetail<JobExecute>(jobData.JobName, jobData.JobGroup, jobData.DeepCloneByXML());

                    await scheduler.ScheduleJob(jobDetail, trigger);
                }
                catch
                {

                }
            }
            await scheduler.Start();
            return true;

        }

        #endregion

        #region 修改任务

        public async Task<bool> EditJob(int id)
        {
            if (await StopJob(id))
            {
                return await Start(id);
            }
            return false;
        }

        #endregion

        #region 停止任务
        public async Task<bool> StopJob(int id)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            var jobData = await JobHelper.GetAutoJobAsync(id);
            if (await Exists(jobData.JobName, jobData.JobGroup))
            {
                return await scheduler.DeleteJob(new JobKey(jobData.JobName, jobData.JobGroup));
            }
            return true;
        }
        #endregion

        #region 检查是否存在

        public async Task<bool> Exists(string name, string group)
        {
            var scheduler = await SchedulerFactory.GetScheduler();
            return await scheduler.GetJobDetail(new JobKey(name, group)) != null;
        }

        #endregion

        #region 停止所有任务

        public async Task<bool> StopAll()
        {
            var scheduer = await SchedulerFactory.GetScheduler();
            await scheduer.PauseAll();
            return true;
        }

        #endregion
    }
}
