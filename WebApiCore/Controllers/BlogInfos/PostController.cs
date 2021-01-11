using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Lib.Model;

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
