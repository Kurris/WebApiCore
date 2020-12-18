using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApiCore.EF
{
    /// <summary>
    /// 自定义数据库拦截器
    /// </summary>
    internal class DbCommandCustomInterceptor : DbCommandInterceptor
    {
        //重写拦截方式
    }
}