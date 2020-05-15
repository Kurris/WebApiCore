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
                // ʹ�÷����ȡxml�ļ�����������ļ���·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory , xmlFile);
                // ����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
                option.IncludeXmlComments(xmlPath , true);
#endif
            });
            #endregion

            services.AddControllers();

            //�Զ�������
            services.AddMvc(option =>
            {
                option.Filters.Add<CustomExceptionFilterAttribute>();
                option.Filters.Add<CustomResourceFilterAttribute>();
                option.Filters.Add(typeof(CustomActionFilterAttribute));
            });


            #region ��������

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

            #region ���� ������UseRouting֮��,UseAuthorization֮ǰ
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
