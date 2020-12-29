using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Data.EF.DataBase;
using WebApiCore.Data.Entity;
using WebApiCore.Data.Entity.BlogInfos;


namespace ApiUnitTest
{
    [TestClass]
    public class Test2_Insert
    {

        public IDataBaseOperation Interface { get => Test1_ConnectionAndDBGenerate.GetInterface(); }

        [TestMethod]
        public async Task Insert1_Single()
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
                        Gender = Gender.Male,
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
        public async Task Insert2_Multi()
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
                        Gender = Gender.Female,
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
                        Gender = Gender.Female,
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

        [TestMethod]
        public async Task Insert3_5000()
        {
            var op = await Interface.BeginTransAsync();
            try
            {
                for (int i = 0; i < 5000; i++)
                {
                    await op.AddAsync(new Blog()
                    {
                        Url = Guid.NewGuid().ToString()
                    });
                }

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
