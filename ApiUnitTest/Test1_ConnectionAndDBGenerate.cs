using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.EF.DataBase;

namespace ApiUnitTest
{
    [TestClass]
    public class Test1_ConnectionAndDBGenerate
    {
        [TestMethod]
        public static IDataBaseOperation GetInterface()
        {
            return InitDB.Create("SqlServer", "Data Source=.;DataBase=MyBlog;Trusted_Connection=True;")
                .GetIDataBaseOperation();
        }


        [TestMethod]
        public async Task CreateDataBase()
        {
            await GetInterface().EnsureDeletedAsync();
            await GetInterface().EnsureCreatedAsync();
        }
    }
}
