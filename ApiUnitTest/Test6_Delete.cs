using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiCore.Data.EF.DataBase;
using WebApiCore.Data.Entity.BlogInfos;

namespace ApiUnitTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Test6_Delete
    {

        public IDataBaseOperation Interface { get => Test1_ConnectionAndDBGenerate.GetInterface(); }


        [TestMethod]
        public async Task Delete1_Entity()
        {
            Blog blog = new Blog() { BlogId = 5 };


            var op = await Interface.BeginTransAsync();
            try
            {
                await op.DeleteAsync(blog);
                var res = await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Delete2_Entitites()
        {
            Blog blog1 = new Blog() { BlogId = 9 };
            Blog blog2 = new Blog() { BlogId = 10 };

            var op = await Interface.BeginTransAsync();
            try
            {
                await op.DeleteAsync<Blog>(new[] { blog1, blog2 });
                var res = await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Delete3_ByKey()
        {

            var op = await Interface.BeginTransAsync();
            try
            {
                await op.DeleteAsync<Blog>(15);
                var res = await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Delete4_ByKeys()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                await op.DeleteAsync<Blog>(new[] { 19, 21 });
                var res = await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Delete5_Field()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                await op.DeleteAsync<Blog>("Url", "Ligy.site");
                var res = await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Delete6_Exp()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                int id = 11;
                await op.DeleteAsync<Blog>(x => x.BlogId == id);
                var res = await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }
    }
}
