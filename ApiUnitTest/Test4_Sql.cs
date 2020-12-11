using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.EF.DataBase;
using WebApiCore.Entity.BlogInfos;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ApiUnitTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Test4_Sql
    {
        public IDataBaseOperation Interface { get => Test1_ConnectionAndDBGenerate.GetInterface(); }

        [TestMethod]
        public async Task Sql1_RunSqlRow()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                var dt = await op.GetTableAsync("select * from blogs");

                //DbParameter[] dbParameter =
                var dt1 = await op.GetTableAsync("select * from blogs where blogid =@id");
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Sql2_RunSqlInterRow()
        {

        }
    }
}
