using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebApiCore.Core;
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

        /// <summary>
        /// DbContext
        /// </summary>
        /// <param name="provider">数据库引擎</param>
        /// <param name="connStr">连接字符串</param>
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
                optionsBuilder.UseMySql(this._connStr, x => x.CommandTimeout(5));
            }
            else
                throw new NotImplementedException("未知的数据引擎");

            optionsBuilder.AddInterceptors(new DbCommandCustomInterceptor());
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
            var userName = await Operator.Instance.GetCurrent() ?? "System";

            foreach (var item in this.ChangeTracker.Entries())
            {
                if (item.State == EntityState.Added)
                {
                    item.Property("Creator").CurrentValue = userName;
                    item.Property("CreateTime").CurrentValue = DateTime.Now;
                }
                else if (item.State == EntityState.Modified)
                {
                    //Update的情况,创建时间不能修改
                    item.Property("Creator").IsModified = false;
                    item.Property("CreateTime").IsModified = false;

                    item.Property("Modifier").CurrentValue = userName;
                    item.Property("ModifyTime").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
