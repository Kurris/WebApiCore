using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Interface.SystemManage;

namespace WebApiCore.AutoJob
{

    public class JobExcute : IJob
    {
        public ILogger<JobExcute> Logger { get; set; }
        public ISchedulerFactory SchedulerFactory { get; set; }
        public IAutoJobService AutoJobService { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobData = context.JobDetail.JobDataMap["data"] as WebApiCore.Entity.SystemManage.AutoJobTask;

            ICronTrigger CronTrigger = context.Trigger as ICronTrigger;
            if (CronTrigger != null)
            {
                if (!CronTrigger.CronExpressionString.Equals(jobData.CronExpression))
                {
                    CronTrigger.CronExpressionString = jobData.CronExpression;
                    var scheduler = await SchedulerFactory.GetScheduler();
                    await scheduler.RescheduleJob(CronTrigger.Key, CronTrigger);
                    await Task.FromResult($"自动任务{jobData.JobName}有误,已提交到下次执行,cron:{jobData.CronExpression}");
                }
                else
                {
                    string[] args = jobData.ExcuteArgs.Split(',');

                    #region 执行任务

                    switch (jobData.JobType)
                    {
                        //插件任务,必须继承IJobPlugin
                        case JobExecuteType.DLLPlugin:
                            break;



                        //存储过程任务
                        case JobExecuteType.Procedure:
                            _ = AutoJobService.ExecProc(jobData.ExcuteName, args);
                            break;


                        //预定任务
                        case JobExecuteType.Destine:
                            _ = JobHelper.ExcuteDestineJobAsync(jobData.JobName, jobData.JobGroup, args);
                            break;
                    }

                    #endregion
                }
            }
        }
    }
}
