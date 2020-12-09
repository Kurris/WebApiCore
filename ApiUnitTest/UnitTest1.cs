using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.EF.DataBase;
using WebApiCore.Entity.BlogInfos;

namespace ApiUnitTest
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public IDataBaseOperation GetInterface()
        {
            return InitDB.Create("SqlServer","Data Source=.;DataBase=MyBlog;Trusted_Connection=True;")
                .GetIDataBaseOperation();
        }


        [TestMethod]
        public async Task Insert()
        {
           
        }

        [TestMethod]
        public async Task Delete()
        {
          
        }

        private readonly Blog blog = new Blog() { Id = 29 };


        [TestMethod]
        public async Task DeleteById()
        {
            
        }
    }
}
