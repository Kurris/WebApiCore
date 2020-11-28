using Microsoft.EntityFrameworkCore;
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
           
        }

        public DbSet<User> Users { get; set; }
    }
}
