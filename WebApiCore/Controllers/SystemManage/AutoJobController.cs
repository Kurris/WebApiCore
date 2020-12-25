using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions;
using WebApiCore.Business.Service;
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
        public BaseService<AutoJobTask> BaseAutoJob { get; set; }

        [HttpPost]
        public async Task<TData<string>> SaveNewJob([FromBody] AutoJobTask autoJob)
        {
            //return await AutoJobService(autoJob);
            return null;
        }

        [HttpPost]
        public async Task<TData<string>> StopJob(int id)
        {
            return new TData<string>()
            {
                Message = await AutoJobService.StopJob(id),
                Status = Status.Success
            };
        }
    }
}
