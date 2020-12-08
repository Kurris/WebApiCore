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
    internal class DbCommandCustomInterceptor : DbCommandInterceptor
    {
    }
}