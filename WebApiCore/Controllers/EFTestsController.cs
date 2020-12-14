using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Entity.BlogInfos;
using WebApiCore.Interface;
using WebApiCore.Service.SystemManager;
using WebApiCore.Utils;

namespace Ligy.Project.WebApi.Controllers
{
    [ApiAuth]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EFTestsController : ControllerBase
    {

        public IBlogService BlogService { get; set; }

        [HttpGet]
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await BlogService.GetBlogs();
        }

        [HttpGet]
        public async Task<OSInfo> GetOSInfo()
        {
            OSInfoService infoService = new OSInfoService();
            return await infoService.GetOSInfo();
        }
    }
}
