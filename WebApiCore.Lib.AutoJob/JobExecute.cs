using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using WebApiCore.Data.Entity.SystemManage;

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

            string[] args = jobData.ExecuteArgs.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var Tjob = jobData.JobType switch
            {
                JobExecuteType.Destine => JobHelper.ExecuteDestineJobAsync(jobData.JobName, jobData.JobGroup, args),
                JobExecuteType.DLLPlugin => JobHelper.ExecutePluginJobAsync(jobData.ExecuteName, jobData.JobName, jobData.JobGroup, args),
                JobExecuteType.Procedure => JobHelper.ExecuteProcAsync(jobData.ExecuteName),
                JobExecuteType.Sql => JobHelper.ExecuteSqlAsync(jobData.ExecuteName),
                _ => throw new NotImplementedException(),
            };
            await Tjob;

            Logger.LogInformation($"自动任务执行:{jobData.JobType.ToString()}:{jobData.ExecuteName}");
        }
    }
}
