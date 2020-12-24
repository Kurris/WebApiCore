using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Controllers.SystemManage
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutoJobController : ControllerBase
    {

        public IAutoJobService AutoJobService { get; set; }

        [HttpPost]
        public async Task<TData<string>> AddNewJob([FromBody] AutoJobTask autoJob)
        {
            await AutoJobService.AddJob(autoJob);
            return null;
        }

        [HttpPost]
        public async void StopJob()
        {
        }
    }
}
