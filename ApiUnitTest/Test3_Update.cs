using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.EF.DataBase;
using WebApiCore.Entity.BlogInfos;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace ApiUnitTest
{
    [TestClass]
    public class Test3_Update
    {
        public IDataBaseOperation Interface { get => Test1_ConnectionAndDBGenerate.GetInterface(); }

        [TestMethod]
        public async Task Update1_Single()
        {
            Blog blog = new Blog()
            {
                BlogId = 1,
                Url = "baodu.com",
                Profile = new Profile()
                {
                    ProfileId = 1,
                    Age = new Random().Next(0, 100),
                },
                Posts = new List<Post>()
                {
                    new Post()  { PostId=1, Title="first post" },
                }
            };

            using var op = await Interface.BeginTransAsync();
            try
            {
                await op.UpdateAsync(blog);
                await op.CommitTransAsync();
            }
            catch (Exception e)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Update2_Multi()
        {
            Blog blog = new Blog()
            {
                BlogId = 1,
                Url = "baodu.com",
                Profile = new Profile()
                {
                    ProfileId = 1,
                    Age = new Random().Next(0, 100),
                },
                Posts = new List<Post>()
                {
                    new Post()  { PostId=1, Title="first post" },
                }
            };

            Blog blog2 = new Blog()
            {
                BlogId = 2,
                Url = "msdn.com",
                Profile = new Profile()
                {
                    ProfileId = 2,
                    Age = new Random().Next(0, 100),
                    Email = "791444095"
                },
                Posts = new List<Post>()
                {
                    new Post()  { PostId=2, Title="learn string finish" },
                }
            };

            using var op = await Interface.BeginTransAsync();
            try
            {
                await op.UpdateAsync<Blog>(new[] { blog, blog2 });
                await op.CommitTransAsync();
            }
            catch (Exception e)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }
    }
}
