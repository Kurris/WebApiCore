﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity.BlogInfos;

namespace WebApiCore.EF
{
    public class ConfigModel
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasOne(x => x.Profile)
                .WithOne(x => x.Blog)
                .HasForeignKey<Profile>("BlogId");

            modelBuilder.Entity<Blog>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.Blog)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Profile>()
                .Property(x => x.Gender)
                .HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v)
                );
        }
    }
}