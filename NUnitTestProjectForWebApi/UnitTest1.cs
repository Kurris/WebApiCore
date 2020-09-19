using NUnit.Framework;
using WebApiCore.Entity;

namespace NUnitTestProjectForWebApi
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            MyDbContext dbContext = new MyDbContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}