using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity.SystemManage;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Controllers.SystemManage
{
    [ApiAuth]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutoJobController : ControllerBase
    {

        public IAutoJobService AutoJobService { get; set; }

        #region 数据操作
        [HttpPost]
        public async Task<TData<int>> SaveNewJob([FromBody] AutoJobTask autoJob)
        {
            return await AutoJobService.SaveAsync(autoJob);
        }

        [HttpGet]
        public async Task<TData<IEnumerable<AutoJobTask>>> FindListJob()
        {
            return await AutoJobService.FindListAsync(null);
        }

        #endregion

        #region JobCenter

        [HttpPost("{id}")]
        public async Task<TData<int>> Start(int id)
        {
            var td = await AutoJobService.SaveAsync(new AutoJobTask()
            {
                AutoJobTaskId = id,
                JobStatus = 1
            });
            if (td.Status == Status.Success)
            {
                td.Message = await AutoJobService.StartJob(id);
            }
            return td;
        }

        [HttpPost("{id}")]
        public async Task<TData<string>> StopJob(int id)
        {
            return new TData<string>()
            {
                Message = await AutoJobService.StopJob(id),
                Status = Status.Success
            };
        }

        [HttpPost("{id}")]
        public async Task<TData<string>> Restart(int id)
        {
            return new TData<string>()
            {
                Message = await AutoJobService.RestartJob(id),
                Status = Status.Success
            };
        }

        #endregion
    }
}
