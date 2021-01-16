using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.CustomClass;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Controllers.SystemManage
{
    [ApiAuth]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OSController : ControllerBase
    {

        [HttpGet]
        public async Task<object> GetSystemInfo()
        {
            return null;
        }
    }
}
