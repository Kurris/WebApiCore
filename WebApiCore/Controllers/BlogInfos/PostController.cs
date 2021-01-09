using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions.BlogInfos;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Controllers.BlogInfos
{
    [ApiAuth]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        public IPostService PostService { get; set; }

        [HttpPost("{id}")]
        public async Task<TData<string>> DeletePost(int id)
        {
            var td = await PostService.DeleteAsync(id);
            return td;
        }
    }
}
