using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity.BlogInfos;


namespace WebApiCore.Business.Service.BlogInfos
{
    public class BlogService : BaseService<Blog>,IBlogService
    {

    }
}
