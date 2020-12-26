using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.CustomClass;

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
    public class ChatHub :Hub
    {
        /// <summary>
        /// Method to be call by frontend
        /// </summary>
        /// <returns></returns>
        public async Task Dosomething()
        {
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(3000);
                await Clients.All.SendAsync("ReceiveUpdate", new Random().Next(0, 10));
            }

            await Clients.All.SendAsync("Done");
        }

        public override async Task OnConnectedAsync()
        {
            string connId = Context.ConnectionId;
            await Clients.Client(connId).SendAsync("someFunc");
        }
    }
}
