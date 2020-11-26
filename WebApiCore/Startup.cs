using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

            services.AddControllers();

            //自定义特性
            services.AddMvc(option =>
            {
                option.Filters.Add<CustomExceptionFilterAttribute>();
                option.Filters.Add<CustomResourceFilterAttribute>();
                option.Filters.Add<CustomActionAndResultFilter>();
            });

            #region 跨域问题

            services.AddCors(option =>
            {
                option.AddPolicy("AllowCors", builder =>
               {
                   builder.AllowAnyOrigin().AllowAnyMethod();
               });
            });

            #endregion


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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/V1/swagger.json", "Ligy.Project.WebApi");
            });
            #endregion

            app.UseRouting();

            #region 跨域 必须在UseRouting之后,UseAuthorization之前
            app.UseCors("AllowCors");
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
