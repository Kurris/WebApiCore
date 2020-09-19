using Microsoft.EntityFrameworkCore;
using System;
using WebApiCore.Entity.Models;

namespace WebApiCore.Entity
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }

        private readonly string _mConnString = @"server=.;database=WebApiDemo;Trusted_Connection=true;";

        public DbSet<UserInfoModel> UserInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_mConnString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfoModel>().HasData(
                new UserInfoModel()
                {
                    UserId = 1,
                    UserName = "Li",
                    Age = 23
                }, new UserInfoModel()
                {
                    UserId = 2,
                    UserName = "Wang",
                    Age = 24
                }
                );
        }
    }
}
