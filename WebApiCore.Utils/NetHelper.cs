using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApiCore.Utils.Browser;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Utils
{
    public class NetHelper
    {
        /// <summary>
        /// Http请求上下文
        /// </summary>
        public static HttpContext HttpContext
        {
            get => GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext;
        }

        public static string UserAgent
        {
            get
            {
                return HttpContext?.Request?.Headers["User-Agent"];
            }
        }

        public static string Browser
        {
            get
            {
                var agent = UserAgent.ParseToString();
                return BrowserHelper.GetBrwoserInfo(agent);
            }
        }
    }
}
