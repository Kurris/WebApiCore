using Microsoft.EntityFrameworkCore;
using System;
using WebApiCore.Entity.BlogInfos;
using WebApiCore.Entity.SystemManage;

namespace WebApiCore.EF
{
    public class ConfigModel
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasOne(x => x.Profile)
                .WithOne(x => x.Blog)
                .HasForeignKey<Profile>("BlogId")
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName).IsUnique();
        }
    }
}
