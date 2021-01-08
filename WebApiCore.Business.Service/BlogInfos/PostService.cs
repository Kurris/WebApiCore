using System;
using System.Collections.Generic;
using System.Text;
using WebApiCore.Business.Abstractions.BlogInfos;
using WebApiCore.Data.Entity;

namespace WebApiCore.Business.Service.BlogInfos
{
   public class PostService :BaseService<Post> ,IPostService
    {
    }
}
