using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var op = InitDB.Create("MySql", "data source=localhost;database=MyBlog; uid=root;pwd=Sa123456!;")
                        .GetIDataBaseOperation();

            await op.EnsureDeletedAsync();
            await op.EnsureCreatedAsync();


            await GetInterface().EnsureDeletedAsync();
            await GetInterface().EnsureCreatedAsync();
        }
    }
}
