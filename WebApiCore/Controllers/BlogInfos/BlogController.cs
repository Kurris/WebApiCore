using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.CustomClass;
using WebApiCore.Entity.BlogInfos;
using WebApiCore.Interface.BlogInfos;
using WebApiCore.Utils;

namespace WebApiCore.Controllers.BlogInfos
{
    [ApiAuth]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
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
            //OSInfoService infoService = new OSInfoService();
            //return await infoService.GetOSInfo();
            return null;
        }
    }
}
