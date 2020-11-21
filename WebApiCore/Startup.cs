using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
            #region Swagger UI
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Ligy.Project.WebApi",
                    Version = "version-01",
                    Description = ".net core webapi by ligy",

                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Ligy",
                        Email = "Ligy.97@foxmail.com"
                    }
                });

#if DEBUG
                //// ʹ�÷����ȡxml�ļ�����������ļ���·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //// ����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
                option.IncludeXmlComments(xmlPath, true);
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
                option.AddPolicy("AllowCors", builder =>
               {
                   builder.AllowAnyOrigin().AllowAnyMethod();
               });
            });

            #endregion


            services.AddDbContext<MyDbContext>(op =>
            {
                op.UseLoggerFactory(LoggerFactory.Create(builder =>
                {
                    builder.AddFilter((string category, LogLevel level) =>
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
