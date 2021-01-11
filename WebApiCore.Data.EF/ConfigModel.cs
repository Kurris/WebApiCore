using System;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Data.Entity;


namespace WebApiCore.Data.EF
{
    public class ConfigModel
    {
        public void Build(ModelBuilder modelBuilder)
        {

            #region Profiles

            modelBuilder.Entity<Profile>()
              .Property(x => x.Gender)
              .HasConversion(
              v => v.ToString(),
              v => (Gender)Enum.Parse(typeof(Gender), v)
              );

            modelBuilder.Entity<Profile>()
                .HasData(new Profile()
                {
                    ProfileId = 1,
                    CreateTime = DateTime.Now,
                    Creator = "ligy",
                    AvatarUrl = "https://avatars3.githubusercontent.com/u/42861557?s=460&u=bea03f68386386ea61fc88c76f27c8db90b509fc&v=4",
                    Email = "Ligy.97@foxmail.com",
                    Age = 23,
                    Gender = Gender.Male,
                    Name = "ligy",
                    Phone = "13790166319",
                    GithubUrl = "https://github.com/Kurris",
                });

            #endregion

            #region Users


            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName).IsUnique();

            modelBuilder.Entity<User>()
                .HasData(new User()
                {
                    UserId = 1,
                    UserName = "ligy",
                    Password = "546677201aae8c8cb69893a4a30d4464",
                    Phone = "13790166319",
                    Email = "Ligy.97@foxmail.com",
                    Creator = "System",
                    CreateTime = DateTime.Now
                });
            #endregion

            #region AutoJobTaks
            modelBuilder.Entity<AutoJobTask>().HasIndex(x => new { x.JobName, x.JobGroup }).IsUnique();
            #endregion

        }
    }
}
