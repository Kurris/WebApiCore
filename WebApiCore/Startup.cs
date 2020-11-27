using Autofac;
using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using WebApiCore.Entity;

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
            services.AddControllers();

            services.AddSwaggerGen(setupAction =>
               {
                   setupAction.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo()
                   {
                       Version = "Ver 1",
                       Description = "Ligy WebApi",
                       Title = "Ligy WebApi",
                       Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                       {
                           Email = "Ligy.97@foxmail.com",
                           Name = "Ligy",
                       }
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

            services.AddDbContext<MyDbContext>(op =>
            {
                op.UseLoggerFactory(
                    LoggerFactory.Create(builder =>
                {
                    builder.AddFilter(
                        (string category, LogLevel level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information
                                      ).AddConsole();

                })).UseMySql(Configuration.GetConnectionString("MySqlDB"));
            });
        }

        /// <summary>
        /// 依赖注入Autofac
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            Assembly service = Assembly.Load("WebApiCore.IOC.Service");
            Assembly iservice = Assembly.Load("WebApiCore.IOC.Interface");
         
            containerBuilder.RegisterAssemblyTypes(service,iservice)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .PropertiesAutowired();
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
