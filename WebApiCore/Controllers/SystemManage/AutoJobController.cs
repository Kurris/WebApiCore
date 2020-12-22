using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.AutoJob;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Controllers.SystemManage
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutoJobController : ControllerBase
    {

        public IAutoJobService AutoJobService { get; set; }
        public IJobCenter JobCenter { get; set; }

        [HttpPost]
        public async Task<TData<string>> AddNewJob([FromBody] WebApiCore.Entity.SystemManage.AutoJob autoJob)
        {
            await AutoJobService.AddJob(autoJob);
            await JobCenter.Add(autoJob.Name, autoJob.Group);
            return null;
        }


        [HttpPost]
        public async Task<TData<string>> StopJob(WebApiCore.Entity.SystemManage.AutoJob autoJob)
        {
            return await AutoJobService.AddJob(autoJob);
        }
    }
}
