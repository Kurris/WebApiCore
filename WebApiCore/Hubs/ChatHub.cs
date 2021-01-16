using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.CustomClass;
using WebApiCore.Lib.Utils;

/*npm install @microsoft/signalr
 *import * as signalR from '@microsoft/signalr'
 * 
 * const connection = new signalR.HubConnectionBuilder()
 *.withUrl("http://localhost:5000/api/chat")
 *.configureLogging(signalR.LogLevel.Information)
 *.build();
 * 
 * connection.on('ClientMethod', x => {
 * 
 * });
 * 
 * connection.start()
 * 
 * 
 * connection.invoke('ServerMethod');
 * 
 * 
 */

namespace WebApiCore.Hubs
{
    [ApiAuth]
    public class ChatHub : Hub
    {
        /// <summary>
        /// Method to be call by frontend
        /// </summary>
        /// <returns></returns>
        public async Task GetCpuValue()
        {
            while (true)
            {
                try
                {
                    await Clients.Caller.SendAsync("setCpuValue", OSHelper.GetCPURate());
                }
                catch
                {
                }
                await Task.Delay(1500);
            }
        }

        public async Task GetMemoryValue()
        {
            while (true)
            {
                try
                {
                    await Clients.Caller.SendAsync("setMemoryValue", OSHelper.GetOSRunTime());
                }
                catch
                {
                }
                await Task.Delay(1000);
            }
        }
    }
}
