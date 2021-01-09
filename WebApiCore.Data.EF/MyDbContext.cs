using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using WebApiCore.Core;
using WebApiCore.Data.EF.DataBase;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Data.EF
{
    public sealed class MyDbContext : DbContext, IDisposable
    {


        //依赖注入则将OnConfiguring方法添加到Startup的ConfigureServices
        //public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        //{
        //}

        private readonly string _provider;
        private readonly string _connStr;


        public MyDbContext()
        {
            this._provider = GlobalInvariant.SystemConfig.DBConfig.Provider;
            if (this._provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                this._connStr = GlobalInvariant.SystemConfig.DBConfig.SqlServerConnectionString;
            }
            else if (this._provider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
            {
                this._connStr = GlobalInvariant.SystemConfig.DBConfig.MySqlConnectionString;
            }
        }

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
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder.AddFilter((string category, LogLevel level) =>
                category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information).AddConsole();
            }));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ConfigModel().Build(modelBuilder);

            var entityTypes = Assembly.Load(new AssemblyName("WebApiCore.Data.Entity"))
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

            var DateTimeConverter = new ValueConverter<DateTime, DateTime>(
               v => v.ToString("yyyy-MM-dd HH:mm:ss").ParseToDateTime(),
               v => v.ToString("yyyy-MM-dd HH:mm:ss").ParseToDateTime()
               );

            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in item.GetProperties().Where(x => x.ClrType == typeof(DateTime) || x.ClrType == typeof(DateTime?)))
                {
                    prop.SetValueConverter(DateTimeConverter);
                    prop.SetMaxLength(14);
                }
            }

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
