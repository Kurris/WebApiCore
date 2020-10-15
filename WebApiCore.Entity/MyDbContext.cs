﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebApiCore.Entity.Models;

namespace WebApiCore.Entity
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WishOrder>().HasKey(x => new { x.GameId, x.UserId });

            modelBuilder.Entity<User>()
                .HasOne(x => x.HomePage)
                .WithOne(x => x.User)
                .HasForeignKey<HomePage>(x => x.UserId);
        }


        public DbSet<Game> Games { get; set; }
        public DbSet<WishOrder> WishOrders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HomePage> HomePages { get; set; }
    }
}
