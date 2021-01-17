using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.CustomClass;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Hubs
{
    [ApiAuth]
    public class SystemHub : Hub
    {
        public async Task GetOSMetrics()
        {
            while (true)
            {
                try
                {
                    var os = OSHelper.GetOSMetrics();
                    await Clients.Caller.SendAsync("setOSMetrics", os);
                }
                catch
                {
                }
                await Task.Delay(1500);
            }
        }
    }
}
