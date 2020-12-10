using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.Entity.BlogInfos;
using WebApiCore.Interface;
using WebApiCore.Utils.Model;

namespace WebApiCore.Service
{
    public class BlogService : IBlogService
    {
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            //var page = new PaginationParam();


            return null;
        }
    }
}
