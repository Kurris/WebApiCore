using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.EF.DataBase;
using WebApiCore.Entity.BlogInfos;
using WebApiCore.Interface;

namespace WebApiCore.Service
{
    public class BlogService : IBlogService
    {
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await new SqlServerDB().FindListAsync<Blog>();
        }
    }
}
