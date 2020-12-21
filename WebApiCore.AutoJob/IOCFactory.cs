using Quartz;
using Quartz.Spi;
using System;

namespace WebApiCore.AutoJob
{
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
            return _serviceProvider.GetService(jobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
