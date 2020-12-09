using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    /// <summary>
    /// MySql数据库
    /// </summary>
    internal class MySqlDB : BaseDatabase
    {
        internal MySqlDB(string provider, string connStr) : base(provider, connStr)
        {
        }

        /*----------------------------------------------重写基类默认的sql行为---------------------------------------------------*/
    }
}
