using Microsoft.EntityFrameworkCore;
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
                .OwnsOne(x => x.Profile)
                .WithOwner(x => x.Blog);

            modelBuilder.Entity<Blog>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.Blog)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
