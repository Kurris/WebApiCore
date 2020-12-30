using System;
using Quartz;
using Quartz.Spi;

namespace WebApiCore.AutoJob
{
    /// <summary>
    /// 自定义的IOC工厂
    /// </summary>
    public class IOCFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public IOCFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobType = bundle.JobDetail.JobType;
            //依赖注入任务
            return _serviceProvider.GetService(jobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            //销毁任务
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
