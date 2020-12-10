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
        public async Task UpdateEntity()
        {

            Blog blog = new Blog()
            {
                BlogId = 1,
                Url = Guid.NewGuid().ToString(),
                Profile = new Profile()
                {
                    ProfileId = 1,
                    Age = 26,
                    Name = "Ligy",
                    Gender = Gender.男,
                    Email = "Ligy.97@foxmail.com",
                },
            };

            using var op = await Interface.BeginTransAsync();
            try
            {
                await op.AttachAsync(blog.Profile, "Age");
                await op.AttachAsync(blog, "Url");
                
                await op.CommitTransAsync();
            }
            catch (Exception)
            {
                await op.CommitTransAsync();
                throw;
            }
        }
    }
}
