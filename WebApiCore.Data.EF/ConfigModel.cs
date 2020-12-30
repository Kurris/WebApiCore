using System;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Data.Entity.BlogInfos;
using WebApiCore.Data.Entity.SystemManage;


namespace WebApiCore.Data.EF
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

            modelBuilder.Entity<AutoJobTask>().HasIndex(x => new { x.JobName, x.JobGroup }).IsUnique();
        }
    }
}
