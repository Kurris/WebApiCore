using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.Entity.BlogInfos;
using WebApiCore.Interface.BlogInfos;

namespace WebApiCore.Service.BlogInfos
{
    public class BlogService : IBlogService
    {
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            //var page = new PaginationParam();

            return await InitDB.Create().FindListAsync<Blog>();
        }
    }
}
