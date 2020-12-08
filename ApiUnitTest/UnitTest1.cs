using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using WebApiCore.EF.DataBase;
using WebApiCore.Entity.BlogInfos;

namespace ApiUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private int delete = 0;

        [TestMethod]
        public async Task<IDataBaseOperation> GetInterface()
        {
            var sqlServer = new SqlServerDB("Data Source=.;DataBase=LigyApi;Trusted_Connection=True;");
            if (delete == 1)
            {
                await sqlServer.GetDataBase().EnsureDeletedAsync();
                delete = 0;
            }

            return await sqlServer.GetDataBase().EnsureCreatedAsync();

        }


        [TestMethod]
        public async Task Insert()
        {
            var db = await GetInterface();
            await db.BeginTransAsync();
            for (int i = 0; i < 1000; i++)
            {
                await db.InsertAsync<Blog>(new Blog()
                {
                    BlogType = BlogType.CSDN,
                    Url = "baidu.com"
                });
            }
            await db.CommitTransAsync();
        }

        [TestMethod]
        public async Task Delete()
        {
            var db = await GetInterface();
            try
            {
                await db.BeginTransAsync();
                int res = await db.DeleteAsync<Blog>(blog);
                res = await db.CommitTransAsync();
            }
            catch (System.Exception)
            {
                await db.RollbackTransAsync();
            }
        }

        private readonly Blog blog = new Blog() { Id = 29 };


        [TestMethod]
        public async Task DeleteById()
        {
            var db = await GetInterface();
            try
            {
                await db.BeginTransAsync();
                int res = await db.DeleteAsync<Blog>(900);
                res = await db.CommitTransAsync();
            }
            catch (System.Exception)
            {
                await db.RollbackTransAsync();
            }
        }
    }
}
