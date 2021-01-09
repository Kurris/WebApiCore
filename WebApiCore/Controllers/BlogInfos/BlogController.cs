using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity;
using WebApiCore.Data.Entity.BlogInfos;
using WebApiCore.Lib.Utils.Extensions;
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
        public async Task<TData<int>> SaveBlog([FromBody] Blog blog)
        {
            return await BlogService.SaveAsync(blog);
        }

        [HttpGet]
        public async Task<TData<object>> GetBlogWithPagniation(string userName, int pageSize, int pageIndex)
        {
            var td = new TData<object>();
            var tdblog = await GetBlog(userName);
            if (tdblog.Status == Status.Success)
            {
                var blog = tdblog.Data;
                var posts = blog.Posts;

                int count = posts.Count;
                var currentData = posts?.Skip(pageSize * (pageIndex - 1)).Take(pageSize)?.ToList();

                td.Data = new
                {
                    count = count,
                    blog = blog,
                    posts = currentData,
                };
                td.Status = Status.Success;
                td.Message = "查询成功";
            }
            else
            {
                td.Status = tdblog.Status;
                td.Message = tdblog.Message;
            }

            return td;
        }



        [HttpGet]
        public async Task<TData<Blog>> GetBlog(string userName)
        {
            var td = new TData<Blog>();
            if (userName.IsEmpty())
            {
                td.Status = Status.Fail;
                td.Message = "用户名称不能为空";
            }
            else
            {
                td.Data = await BlogService.DbContext.Set<Blog>().Include(x => x.Posts)
                                                            .ThenInclude(x => x.Comments).Where(x => x.UserName == userName).FirstOrDefaultAsync();
                td.Status = Status.Success;
                td.Message = "获取成功";
            }
            return td;
        }


        [HttpGet]
        public async Task<TData<Blog>> GetPost(int blogId, int postId)
        {
            var td = new TData<Blog>();
            var blog = await BlogService.DbContext.Set<Blog>().Include(x => x.Posts)
                                                            .ThenInclude(x => x.Comments).Where(x => x.BlogId == blogId).FirstOrDefaultAsync();

            List<Post> post;
            if (postId == 0)
            {
                post = new List<Post>();
                post.Add(blog?.Posts?.ToList().LastOrDefault());
            }
            else
                post = blog?.Posts?.Where(x => x.PostId == postId).ToList();
            
            if (post == null)
            {
                td.Message = "查询失败";
                td.Status = Status.Fail;
            }
            else
            {
                blog.Posts = post;
                td.Data = blog;
                td.Message = "查询成功";
                td.Status = Status.Success;
            }
            return td;
        }
    }
}
