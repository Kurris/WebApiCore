using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

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