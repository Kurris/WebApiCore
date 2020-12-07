using Autofac;
using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApiCore.Entity;
using WebApiCore.Utils;

namespace Ligy.Project.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();

            services.AddSwaggerGen(setupAction =>
               {
                   setupAction.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo()
                   {
                       Version = "Ver 1",
                       Title = "Ligy WebApi",
                   });
               });

            //自定义特性
            services.AddMvc(option =>
            {
                option.Filters.Add<CustomExceptionFilterAttribute>();
                option.Filters.Add<CustomResourceFilterAttribute>();
                option.Filters.Add<CustomActionAndResultFilter>();
            });


            services.AddCors(option =>
            {
                option.AddPolicy("AllowCors", builder =>
               {
                   builder.AllowAnyOrigin().AllowAnyMethod();
               });
            });


            var global = Configuration.GetSection("SystemConfig").Get<GlobalInvariant>();

            services.AddDbContext<MyDbContext>(op =>
            {
                string provider = global.SystemConfig.DBConfig.Provider;

                switch (provider)
                {
                    case "SqlServer":
                        op.UseSqlServer(global.SystemConfig.DBConfig.SqlServerConnectionString);
                        break;




                        throw new System.ArgumentException("未知的数据库引擎!");
                }
            });
        }

        /// <summary>
        /// 依赖注入Autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CustomAutofacModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/V1/swagger.json", "Ligy.Project.WebApi");
            });

            app.UseRouting();

            //跨域 必须在UseRouting之后,UseAuthorization之前
            app.UseCors("AllowCors");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
