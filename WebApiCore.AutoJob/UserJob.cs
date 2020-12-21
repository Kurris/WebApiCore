using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using WebApiCore.Utils;

namespace WebApiCore.AutoJob
{
    public class UserJob : IJob
    {
        public ILogger<UserJob> Logger { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation($"{DateTime.Now}正在同步数据");
        }
    }
}
