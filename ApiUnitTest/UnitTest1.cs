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
    public class UnitTest1
    {

        [TestMethod]
        public IDataBaseOperation GetInterface()
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

        [TestMethod]
        public async Task Insert()
        {
            var blog = new Blog()
            {
                Url = "Ligy.site",
                Profile = new Profile()
                {
                    Age = 23,
                    Name = "Ligy",
                    Gender = Gender.ÄÐ,
                    Email = "Ligy.97@foxmail.com",
                },
                Posts = new List<Post>()
                 {
                        new Post()
                        {
                             Title="first post",
                             Content="Hello MyBlog",
                        }
                 }
            };

            await GetInterface().AddAsync<Blog>(blog);
        }


        [TestMethod]
        public async Task Find()
        {
            var op = await GetInterface().BeginTransAsync();

            try
            {

            }
            catch
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task Update()
        {
            var op = await GetInterface().BeginTransAsync();

            try
            {
                //Blog blog = await op.FindEntityAsync<Blog>(2);
                //blog.Url = Guid.NewGuid().ToString();
                //var a = await op.CommitTransAsync();
            }
            catch (System.Exception)
            {
                await op.RollbackTransAsync();
            }
        }


        [TestMethod]
        public async Task Delete()
        {

        }


        [TestMethod]
        public async Task DeleteById()
        {

        }
    }
}
