using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions.SystemManage;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity.BlogInfos;
using WebApiCore.Lib.Utils.Extensions;
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
            if (name.IsEmpty())
            {
                return new TData<Profile>()
                {
                    Message = "查找名称不能为空",
                    Status = Status.Fail,
                    Data = null
                };
            }
            return await ProfileService.FindAsync(x => x.Name == name);
        }
    }
}
