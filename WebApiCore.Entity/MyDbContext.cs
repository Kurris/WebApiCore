using Microsoft.EntityFrameworkCore;
using System;
using WebApiCore.Entity.Model;
using WebApiCore.Entity.ModelConfig;

namespace WebApiCore.Entity
{
    public sealed class MyDbContext : DbContext
    {
        //public MyDbContext()
        //{

        //}

        //public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        //{

        //}


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("data source=localhost;database=LigyApi; uid=root;pwd=Sa123456!;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            new BlogConfig().Configure(modelBuilder.Entity<Blog>());
            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
