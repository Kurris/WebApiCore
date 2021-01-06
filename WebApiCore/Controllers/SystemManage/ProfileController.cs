using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions.SystemManage;
using WebApiCore.Data.Entity.BlogInfos;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Controllers.SystemManage
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        public IProfileService ProfileService { get; set; }


        [HttpGet]
        public async Task<TData<Profile>> GetProfile(string name)
        {
            return await ProfileService.FindAsync(x => x.Name == name);
        }
    }
}
