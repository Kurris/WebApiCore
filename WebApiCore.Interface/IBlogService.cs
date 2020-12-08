using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity.BlogInfos;

namespace WebApiCore.Interface
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogs();
    }
}
