using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System.Threading.Tasks;

namespace WebApiCore.AutoJob
{
    public class AutoJobCenter : IJobCenter
    {
        public ILogger<AutoJobCenter> Logger { get; set; }
        public ISchedulerFactory SchedulerFactory { get; set; }

        private IScheduler _scheduler = null;

        public IJobFactory IOCFactory { get; set; }

        public async Task<string> Start()
        {
            //1、通过调度工厂获得调度器
            _scheduler = await SchedulerFactory.GetScheduler();
            _scheduler.JobFactory = IOCFactory;

            //2、开启调度器
            await _scheduler.Start();
            //3、创建一个触发器
            var trigger = TriggerBuilder.Create()
                                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                                        .UsingJobData(new JobDataMap())
                                        .Build();
            //4、创建任务
            var jobDetail = JobBuilder.Create<UserJob>()
                            .WithIdentity(new JobKey("user", "SystemManage"))//放进工厂
                            .Build();

            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult("将触发器和任务器绑定到调度器中完成");
        }

        public async Task<string> Stop()
        {
            return await Task.FromResult("已停止自动任务");
        }
    }
}
