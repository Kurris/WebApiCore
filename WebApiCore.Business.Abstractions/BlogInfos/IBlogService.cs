using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Data.Entity.BlogInfos;

namespace WebApiCore.Business.Abstractions
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogs();
    }
}
