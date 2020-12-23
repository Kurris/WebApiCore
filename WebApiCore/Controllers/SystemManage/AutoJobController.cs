using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiCore.AutoJob;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Model;
using WebApiCore.Entity.SystemManage;
using System.Threading;
using System.Net.WebSockets;
using System.Text;
using System.Diagnostics;

namespace WebApiCore.Controllers.SystemManage
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutoJobController : ControllerBase
    {

        public IAutoJobService AutoJobService { get; set; }
        public IJobCenter JobCenter { get; set; }

        [HttpPost]
        public async Task<TData<string>> AddNewJob([FromBody] AutoJobTask autoJob)
        {
            await AutoJobService.AddJob(autoJob);
            await JobCenter.AddNewJob(autoJob.JobName, autoJob.JobGroup);
            return null;
        }

        [Route("{jobid?}")]
        [HttpPost]
        public async Task<TData<string>> StopJob()
        {

            return null;
        }


        #region unuse
        //[HttpGet]
        //public async Task GetTask()
        //{
        //    if (HttpContext.WebSockets.IsWebSocketRequest)
        //    {
        //        var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        //        Res(webSocket);
        //        await SendEvent(webSocket);
        //        await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
        //    }
        //}

        //private async Task Res(WebSocket webSocket)
        //{

        //    var arr = new System.ArraySegment<byte>(new byte[4 * 1024]);

        //    var result = await webSocket.ReceiveAsync(arr, CancellationToken.None);
        //    while (!result.EndOfMessage)
        //    {
        //        result = await webSocket.ReceiveAsync(arr, CancellationToken.None);
        //    }
        //    Debug.WriteLine(Encoding.UTF8.GetString(arr.Array));
        //}

        //private async Task SendEvent(WebSocket webSocket)
        //{
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        await Task.Delay(1000);
        //        await webSocket.SendAsync(new System.ArraySegment<byte>(Encoding.ASCII.GetBytes("hello", 0, "hello".Length)), WebSocketMessageType.Text, true, CancellationToken.None);
        //    }
        //} 
        #endregion

        [HttpGet]
        public async Task TestSignalr()
        {

        }
    }
}
