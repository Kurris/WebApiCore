using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApiCore.Lib.Utils.Browser;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Lib.Utils
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
                var agent = UserAgent.ParseToStr();
                return BrowserHelper.GetBrwoserInfo(agent);
            }
        }
    }
}
