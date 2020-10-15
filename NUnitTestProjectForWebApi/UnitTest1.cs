using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Linq;
using WebApiCore.Entity;
using WebApiCore.Entity.Models;

namespace NUnitTestProjectForWebApi
{
    public class Tests
    {
        [Test]
        public void AddUser()
        {
            using TestDbContext testDbContext = new TestDbContext();
            var user = new User()
            {
                Name = "Ting",
                Age = 22,

            };
            testDbContext.Users.Add(user);
            testDbContext.Users.Remove(user);

            testDbContext.SaveChanges();
        }

        [Test]
        public void AddGame()
        {
            using var testDbContext = new TestDbContext();

            var game = new Game()
            {
                Name = "2077",
                Price = 400,
                Publish = new DateTime(2020, 11, 19),
            };

            testDbContext.Games.Add(game);
            testDbContext.Games.Remove(game);

            testDbContext.SaveChanges();
        }

        [Test]
        public void AddWishOrder()
        {
            using var testDbContext = new TestDbContext();

            var user = testDbContext.Users.Single();
            var game = testDbContext.Games.Single();

            var wd = new WishOrder()
            {
                Game = game,
                User = user
            };

            testDbContext.WishOrders.Add(wd);
            testDbContext.WishOrders.Remove(wd);
            testDbContext.SaveChanges();
        }

        [Test]
        public void QueryUserWishOrder()
        {
            using var testDbContext = new TestDbContext();
            var user = testDbContext.Users
                        .Include(x => x.WishOrders)
                            .ThenInclude(x => x.Game)
                        .Single();
        }

        [Test]
        public void AddUserHomepage()
        {
            using var db = new TestDbContext();

            var user = db.Users.Include(x => x.HomePage).Single();

            user.HomePage = new HomePage()
            {
                Country = "CHINA",
                City = "Dongguan Province",
                Introduction = "唔好理,就系好犀利!"
            };

            db.SaveChanges();
        }

        [Test]
        public void QueryUserAll()
        {
            using var testDbContext = new TestDbContext();
            var user = testDbContext.Users
                .Include(x => x.HomePage)
                        .Include(x => x.WishOrders)
                            .ThenInclude(x => x.Game)
                        .Single();
        }
    }
}