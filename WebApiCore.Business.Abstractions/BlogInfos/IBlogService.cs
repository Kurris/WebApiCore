using System.Threading.Tasks;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Model;

namespace WebApiCore.Business.Abstractions
{
    public interface IBlogService : IBaseService<Blog>
    {
        Task<TData<object>> GetBlogWithPagination(string userName, Pagination pagination);
        Task<TData<Blog>> GetCurrentPost(int blogId, int postId);
    }
}
