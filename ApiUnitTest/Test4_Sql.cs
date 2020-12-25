using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Data.EF.DataBase;
using WebApiCore.Data.Entity;
using WebApiCore.Data.Entity.BlogInfos;

namespace ApiUnitTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Test4_Sql
    {
        public IDataBaseOperation Interface { get => Test1_ConnectionAndDBGenerate.GetInterface(); }

        [TestMethod]
        public async Task Sql1_GetDataTable()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                var dt = await op.GetTableAsync("select * from blogs");

                IDictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    ["id"] = 2
                };
                var dt1 = await op.GetTableAsync("select * from blogs where blogid =@id", parameters);
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Sql2_CreateTable()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                string sql = @"
if object_id('test1111') is not null
drop table test1111

CREATE table test1111
(
id int identity(1,1)  primary key  not null,
name nvarchar(10) not null
)";


                await op.RunSqlAsync(sql);
                await op.RunSqlAsync("insert into test1111(name) values('ligy')");

                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Sql3_RunSqlRow()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                await op.RunSqlAsync("insert into test1111(name) values('xiao')");

                IDictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    ["name"] = "明"
                };
                var dt1 = await op.RunSqlAsync("insert into test1111(name) values(@name)", parameters);
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }
        [TestMethod]
        public async Task Sql4_RunSqlInterRow()
        {
            var op = await Interface.BeginTransAsync();
            try
            {

                var dt1 = await op.RunSqlInterAsync($"insert into test1111(name) values({"键"})");
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Sql5_RunProc()
        {


            var op = await Interface.BeginTransAsync();
            try
            {
                await op.RunSqlAsync(@"
if object_id('testp') is not null
drop PROCEDURE testp
");

                string proc = @"

CREATE  PROCEDURE testp @id int as
	delete from test1111 where id=@id


";
                await op.RunSqlAsync(proc);

                await op.ExecProcAsync("testp @id=@pid", new Dictionary<string, object>()
                {
                    ["pid"] = 3
                });

                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }


        [TestMethod]
        public async Task Sql6_GetReader()
        {

            var op = await Interface.BeginTransAsync();
            try
            {
                using (var reader = await op.GetReaderAsync("select top(1) * from test1111"))
                {

                    //yewuluojio
                }

                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Sql7_GetScalar()
        {

            var op = await Interface.BeginTransAsync();
            try
            {
                object scalar = await op.GetScalarAsync("select  * from test1111");
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Sql9_DropAll()
        {

            var op = await Interface.BeginTransAsync();
            try
            {
                await op.RunSqlAsync(@"
if object_id('testp') is not null
drop PROCEDURE testp
");


                await op.RunSqlAsync(@"
if object_id('test1111') is not null
drop table test1111");

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
