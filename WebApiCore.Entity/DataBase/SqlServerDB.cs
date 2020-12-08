using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    internal class SqlServerDB : BaseDatabase
    {
        internal SqlServerDB(string provider, string connStr) : base(provider, connStr)
        {
        }
    }
}
