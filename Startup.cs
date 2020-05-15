using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ligy.Project.WebApi.CustomClass;

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
            #region Swagger UI
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("V1" , new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Ligy.Project.WebApi" ,
                    Version = "version-01" ,
                    Description = ".net core webapi by ligy" ,

                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Ligy" ,
                        Email = "Ligy.97@foxmail"
                    }
                });

#if DEBUG
                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory , xmlFile);
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                option.IncludeXmlComments(xmlPath , true);
#endif
            });
            #endregion

            services.AddControllers();

            //自定义特性
            services.AddMvc(option =>
            {
                option.Filters.Add<CustomExceptionFilterAttribute>();
                option.Filters.Add<CustomResourceFilterAttribute>();
                option.Filters.Add(typeof(CustomActionFilterAttribute));
            });


            #region 跨域问题

            services.AddCors(option =>
            {
                option.AddPolicy("AllowCors" , builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod();
                });
            });

            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app , IWebHostEnvironment env)
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/V1/swagger.json" , "Ligy.Project.WebApi");
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
