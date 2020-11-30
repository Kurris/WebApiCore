using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Entity;
using WebApiCore.Entity.Model;

namespace WebApiCore.Test.DB
{
    class Program
    {

        static async Task Main(string[] args)
        {
            //using (var db = new MyDbContext())
            //{
            //    db.Blogs.Remove(new Blog() { BlogId = 1 });
            //    db.SaveChanges();
            //}

            //using (var db = new MyDbContext())
            //{
            //    var blog = new Blog { Url = "http://sample.com" };
            //    db.Blogs.Add(blog);
            //    db.SaveChanges();
            //}

            //using (var db = new MyDbContext())
            //{
            //    db.Database.EnsureCreated();

            //    var blogs1 = db.Blogs.Find(2);
            //    blogs1.Rating = 5;

            //    var blogs2 = db.Blogs.Single(x => x.BlogId == 2);
            //    blogs2.Rating = 10;

            //    var @bool = db.Entry(new Blog()
            //    {
            //        Rating = 5,
            //        Url = "baidu.com"
            //    }).IsKeySet;

            //    db.SaveChanges();
            //}

            using (var db = new MyDbContext())
            {
                var blog = db.Blogs.Find(2);
                blog.Rating = 10;
                db.Entry(blog).Property<string>("shadowprop").CurrentValue = "value";

                var a= db.Blogs.OrderBy(x => EF.Property<string>(x, "shadowprop"));
                db.SaveChanges();
            }
        }
    }
}
