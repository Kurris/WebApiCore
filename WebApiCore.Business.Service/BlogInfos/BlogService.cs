using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity.BlogInfos;


namespace WebApiCore.Business.Service.BlogInfos
{
    public class BlogService : IBlogService
    {
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            //var page = new PaginationParam();

            return await EFDB.Instance.FindListAsync<Blog>();
        }
    }
}
