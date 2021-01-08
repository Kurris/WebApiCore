using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions.BlogInfos;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Controllers.BlogInfos
{
    [ApiAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        public IPostService PostService { get; set; }

        [HttpGet]
        public async Task<TData<IEnumerable<Post>>> GetPosts(int blogId)
        {
            return await PostService.FindListAsync(x => x.Blog.BlogId == blogId);
        }


        [HttpPost]
        public async Task<TData<string>> AddPost([FromBody] Post post)
        {
            return await PostService.SaveAsync(post);
        }

        [HttpPost("{id}")]
        public async Task<TData<string>> DeletePost(int postId)
        {
            return await PostService.DeleteAsync(postId);
        }
    }
}
