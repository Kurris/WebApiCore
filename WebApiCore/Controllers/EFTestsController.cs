using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Entity.BlogInfos;
using WebApiCore.Interface;

namespace Ligy.Project.WebApi.Controllers
{
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
    }
}
