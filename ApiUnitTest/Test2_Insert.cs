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
    public class Test2_Insert
    {

        public IDataBaseOperation Interface { get => Test1_ConnectionAndDBGenerate.GetInterface(); }

        [TestMethod]
        public async Task InsertSingleEntity()
        {
            var op = await Interface.BeginTransAsync();
            try
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
                        new Post()  {Title="first post",Content="Hello MyBlog", },
                        new Post()  {Title="second post",Content="learn string", },
                    }
                };
                await op.AddAsync<Blog>(blog);
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.RollbackTransAsync();
                throw;
            }
        }

        [TestMethod]
        public async Task InsertEntitites()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                await op.AddAsync<Blog>(new[]
                   {
   new Blog(){
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
                        new Post()  {Title="first post",Content="Hello MyBlog", },
                        new Post()  {Title="second post",Content="learn string", },
                    }
                },
                            new Blog()
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
                        new Post()  {Title="first post",Content="Hello MyBlog", },
                        new Post()  {Title="second post",Content="learn string", },
                    }
            }

                });

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
