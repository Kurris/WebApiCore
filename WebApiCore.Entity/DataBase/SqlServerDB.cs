using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    public class SqlServerDB : BaseDatabase
    {
        public SqlServerDB()
        {

        }

        public SqlServerDB(string connStr) : base(connStr)
        {
        }
    }
}
