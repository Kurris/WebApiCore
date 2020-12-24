using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiCore.AutoJob;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Model;
using WebApiCore.Entity.SystemManage;

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

        [Area("{jobid?}")]
        [HttpPost]
        public async void StopJob()
        {
        }
    }
}
