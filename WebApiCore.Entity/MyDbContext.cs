using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebApiCore.Entity;
using WebApiCore.Entity.BlogInfos;

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
            optionsBuilder.LogTo(Write, (eveid, level) =>
             {
                 return level == LogLevel.Information;
             });
        }

        private void Write(string log)
        {
            Debug.WriteLine(log);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = Assembly.Load(new AssemblyName("WebApiCore.Entity"))
                .GetTypes().Where(x => x.IsSubclassOf(typeof(BaseEntity))
                               && x.IsDefined(typeof(TableAttribute)));


            foreach (var entityType in entityTypes)
            {
                if (modelBuilder.Model.GetEntityTypes().Any(x => x.Name.Equals(entityType.FullName)))
                {
                    continue;
                }
                modelBuilder.Entity(entityType);
            }

            ConfigModel.Build(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 重写SaveChangesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in this.ChangeTracker.Entries())
            {
                if (item.State == EntityState.Added)
                {
                    item.Property("CreateTime").CurrentValue = DateTime.Now;
                }
                else if (item.State == EntityState.Modified)
                {
                    //Update的情况,创建时间不能修改
                    item.Property("Creator").IsModified = false;
                    item.Property("CreateTime").IsModified = false;

                    item.Property("Modifier").CurrentValue = item.Property("Modifier").CurrentValue ?? "System";
                    item.Property("ModifyTime").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
