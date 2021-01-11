using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Model;

namespace WebApiCore.Controllers.BlogInfos
{
    [ApiAuth]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {

        public IBlogService BlogService { get; set; }

        [HttpPost]
        public async Task<TData<int>> SaveBlog([FromBody] Blog blog)
        {
            return await BlogService.SaveAsync(blog);
        }

        [HttpGet]
        public async Task<TData<object>> GetBlogWithPagniation(string userName, int pageSize, int pageIndex)
        {
            TData<object> tobj = await BlogService.GetBlogWithPagination(userName, new Pagination()
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            });
            return tobj;
        }

        [HttpGet]
        public async Task<TData<Blog>> GetBlogCurrentPost([FromQuery] int blogId, [FromQuery] int postId)
        {
            var td =await BlogService.GetCurrentPost(blogId, postId);
            return td;
        }
    }
}
