using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Lib.Model;
using System.Collections.Generic;
using WebApiCore.Lib.Utils.Extensions;
using WebApiCore.Data.Entity;
using System;

namespace WebApiCore.Business.Service
{
    public class BlogService : BaseService<Blog>, IBlogService
    {
        public async Task<TData<object>> GetBlogWithPagination(string userName, Pagination pagination)
        {
            var td = new TData<object>();
            var blogAll = await GetBlog(userName);
            if (blogAll.Status == Status.Success)
            {
                Blog blog = blogAll.Data;
                IEnumerable<Post> posts = blog.Posts;

                pagination.Total = posts.Count();
                var currentData = posts?.Skip(pagination.PageSize * (pagination.PageIndex - 1)).Take(pagination.PageSize)?.ToList();

                td.Data = new
                {
                    total = pagination.Total,
                    blog,
                    posts = currentData,
                };
                td.Status = Status.Success;
                td.Message = "查询成功";
            }
            else
            {
                td.Status = blogAll.Status;
                td.Message = blogAll.Message;
            }

            return td;
        }

        public async Task<TData<Blog>> GetCurrentPost(int blogId, int postId)
        {
            var td = new TData<Blog>();
            var op = await EFDB.Instance.AsNoTracking().BeginTransAsync();
            Blog blog = null;
            List<Post> posts = null;

            try
            {
                blog = await op.DbContext.Set<Blog>().Include(x => x.Posts)
                                           .ThenInclude(x => x.Comments).Where(x => x.BlogId == blogId).FirstOrDefaultAsync();

                //最新
                if (postId == 0)
                {
                    posts = new List<Post>
                {
                    blog?.Posts?.ToList().LastOrDefault()
                };
                }
                else
                {
                    posts = blog?.Posts?.Where(x => x.PostId == postId).ToList();
                }

                await op.CommitTransAsync();
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                td.Message = ex.GetBaseException().Message;
                return td;
            }

            if (posts == null)
            {
                td.Message = "查询失败";
                td.Status = Status.Fail;
            }
            else
            {
                blog.Posts = posts;
                td.Data = blog;
                td.Message = "查询成功";
                td.Status = Status.Success;
            }
            return td;
        }

        private async Task<TData<Blog>> GetBlog(string userName)
        {
            var td = new TData<Blog>();
            if (userName.IsEmpty())
            {
                td.Status = Status.Fail;
                td.Message = "用户名称不能为空";
            }
            else
            {
                td.Data = await EFDB.Instance.AsNoTracking().DbContext.Set<Blog>().Include(x => x.Posts)
                                                                                      .ThenInclude(x => x.Comments).Where(x => x.UserName == userName)
                                                                                      .FirstOrDefaultAsync();
                td.Status = Status.Success;
                td.Message = "获取成功";
            }
            return td;
        }
    }
}
