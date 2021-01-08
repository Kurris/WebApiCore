using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity.BlogInfos;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Controllers.BlogInfos
{
    [ApiAuth]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {

        public IBlogService BlogService { get; set; }

        [HttpPost]
        public async Task<TData<string>> SaveBlog([FromBody] Blog blog)
        {
            return await BlogService.SaveAsync(blog);
        }

        [HttpGet]
        public async Task<TData<Blog>> GetBlog(string userName)
        {
            return await BlogService.FindAsync(x => x.UserName == userName);
        }
    }
}
