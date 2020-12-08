using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using WebApiCore.Entity;

namespace WebApiCore.EF
{
    public sealed class MyDbContext : DbContext, IDisposable
    {


        //依赖注入则将OnConfiguring方法添加到Startup的ConfigureServices
        //public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        //{
        //}

        private readonly string _provider;
        private readonly string _connStr;

        public MyDbContext(string provider, string connStr)
        {
            this._provider = provider;
            this._connStr = connStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (this._provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                optionsBuilder.UseSqlServer(this._connStr, x => x.CommandTimeout(5));
            }
            else if (this._provider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
            {
                //optionsBuilder.(GlobalInvariant.SystemConfig.DBConfig.MySqlConnectionString);
                throw new NotImplementedException("MySql尚未实现!");
            }
            else
                throw new NotImplementedException("未知的数据引擎");

            optionsBuilder.AddInterceptors(new DbCommandCustomInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Assembly entityAssembly = Assembly.Load(new AssemblyName("WebApiCore.Entity"));
            IEnumerable<Type> typesToRegister = entityAssembly.GetTypes().Where(p => !string.IsNullOrEmpty(p.Namespace))
                                                                         .Where(p => !string.IsNullOrEmpty(p.GetCustomAttribute<TableAttribute>()?.Name))
                                                                         .Where(p => p.IsSubclassOf(typeof(BaseEntity)));
            foreach (Type type in typesToRegister)
            {
                if (modelBuilder.Model.GetEntityTypes().Any(x => x.Name == type.FullName))
                {
                    continue;
                }
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Model.AddEntityType(type);
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                string currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName);

                modelBuilder.Entity(entity.Name)
                    .Property("Id");
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
