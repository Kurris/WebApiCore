using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiCore.Entity.Models;

namespace WebApiCore.Entity
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Persist Security Info=False;User ID=sa;Password=a123456!;Initial Catalog=EFDemo;Data Source=.");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WishOrder>().HasKey(x => new { x.GameId, x.UserId });
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<WishOrder> WishOrders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HomePage> HomePages { get; set; }
    }
}
