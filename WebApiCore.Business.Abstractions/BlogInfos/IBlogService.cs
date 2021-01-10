using WebApiCore.Data.Entity.BlogInfos;

namespace WebApiCore.Business.Abstractions
{
    public interface IBlogService : IBaseService<Blog>
    {
        //  GetBlogWithPagination(string userName, int pageSize, int pageIndex);
    }
}
