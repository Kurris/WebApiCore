using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Entity.BlogInfos;

namespace WebApiCore.Interface.BlogInfos
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogs();
    }
}
