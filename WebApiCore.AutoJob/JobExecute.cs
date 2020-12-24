using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManage;

namespace WebApiCore.AutoJob
{

    /// <summary>
    /// 任务执行
    /// </summary>
    public class JobExecute : IJob
    {
        public ILogger<JobExecute> Logger { get; set; }
        public ISchedulerFactory SchedulerFactory { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobData = context.JobDetail.JobDataMap["data"] as AutoJobTask;

            if (context.Trigger is ICronTrigger CronTrigger)
            {
                if (!CronTrigger.CronExpressionString.Equals(jobData.CronExpression))
                {
                    CronTrigger.CronExpressionString = jobData.CronExpression;
                    var scheduler = await SchedulerFactory.GetScheduler();
                    await scheduler.RescheduleJob(CronTrigger.Key, CronTrigger);

                    Logger.LogWarning("执行任务与数据库不匹配!");
                }
                else
                {
                    string[] args = jobData.ExecuteArgs.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    #region 执行任务

                    switch (jobData.JobType)
                    {
                        //插件任务,必须继承IJobPlugin
                        case JobExecuteType.DLLPlugin:
                            _ = JobHelper.ExecutePluginJobAsync(jobData.ExecuteName, jobData.JobName, jobData.JobGroup, args);
                            break;


                        //存储过程任务
                        case JobExecuteType.Procedure:
                            _ = JobHelper.ExecuteProcAsync(jobData.ExecuteName);
                            break;

                        //sql语句
                        case JobExecuteType.Sql:
                            _ = JobHelper.ExecuteSqlAsync(jobData.ExecuteName);
                            break;

                        //预定任务
                        case JobExecuteType.Destine:
                            _ = JobHelper.ExecuteDestineJobAsync(jobData.JobName, jobData.JobGroup, args);
                            break;
                    }

                    #endregion
                }
            }
        }
    }
}
