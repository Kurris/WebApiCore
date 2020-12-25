using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Data.EF.DataBase;
using WebApiCore.Data.Entity.BlogInfos;


namespace ApiUnitTest
{
    [TestClass]
    public class Test5_Find
    {
        public IDataBaseOperation Interface { get => Test1_ConnectionAndDBGenerate.GetInterface(); }


        [TestMethod]
        public async Task Find1_EntityBySingleKey()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                Blog b1 = await op.FindAsync<Blog>(2);
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Find2_EntityByExp()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                Blog b1 = await op.FindAsync<Blog>(x => x.BlogId == 3);
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Find3_ListByExpOrNot()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                var lisb1 = await op.FindListAsync<Blog>();

                var lisb2 = await op.FindListAsync<Blog>(x => x.Creator == "system");

                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }


        [TestMethod]
        public async Task Find4_ListByOrder()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                var lisb1 = await op.FindListByOrderAsync<Blog>(x => x.BlogId, true);
                var lisb2 = await op.FindListByOrderAsync<Blog>(x => x.BlogId, false);

                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Find5_ListByPagaination()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                var (total, lis) = await op.FindListAsync<Blog>("BlogId", true, 7, 3);

                var (total1, lis1) = await op.FindListAsync<Blog>("BlogId", true, 10, 5);


                var (total2, lis2) = await op.FindListAsync<Blog>(x => x.Creator == "System", "BlogId desc", false, 20, 1);

                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Find6_TableBySQLPagaination()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                IDictionary<string, object> paras = new Dictionary<string, object>()
                {
                    ["m"] = "system"
                };

                var (total2, lis2) = await op.FindTableAsync("select * from blogs where modifier=@m", paras, "blogid", true, 10, 1);


                var (count, table) = await op.FindTableAsync("select * from blogs", null, "blogid", true, 7, 3);

                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }
    }
}
