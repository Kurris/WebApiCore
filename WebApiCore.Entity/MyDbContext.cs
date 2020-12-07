using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
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
            optionsBuilder.UseSqlServer("Data Source=.;DataBase=LigyApi;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Assembly entityAssembly = Assembly.Load(new AssemblyName("WebApiCore.Entity"));
            IEnumerable<Type> typesToRegister = entityAssembly.GetTypes().Where(p => !string.IsNullOrEmpty(p.Namespace))
                                                                         .Where(p => !string.IsNullOrEmpty(p.GetCustomAttribute<TableAttribute>()?.Name));
            foreach (Type type in typesToRegister)
            {
                if (modelBuilder.Model.GetEntityTypes().Any(x=>x.Name==type.FullName))
                {
                    continue;
                }
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Model.AddEntityType(type);
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                SetPrimaryKey(modelBuilder, entity.Name);
                string currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName);
            }

            base.OnModelCreating(modelBuilder);
        }

        public  void SetPrimaryKey(ModelBuilder modelBuilder, string entityName)
        {
            modelBuilder.Entity(entityName).HasKey("Id");
        }
    }
}
