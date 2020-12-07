using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Entity;
using WebApiCore.Entity.Model;
using System.Threading;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebApiCore.Test.DB
{
    class Program
    {

        static void Main(string[] args)
        {
            using (var db = new MyDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();


                db.Set<Blog>().Add(new Blog()
                {
                    BlogType = BlogType.MSDN,
                    Url = "baidu.com"
                });

                db.SaveChanges();
            }
        }
    }
}
