using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity.BlogInfos;


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


    }
}
