using Autofac;
using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Utils;
using WebApiCore.Utils.Extensions;
using WebApiCore.Utils.Model;

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
                       Title = "MyBlog WebApi",
                   });
               });

            //�Զ�������
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

            services.AddMemoryCache();


            GlobalInvariant.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalInvariant.Configuration = Configuration;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(op =>
            {
                op.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        string loginProvider = GlobalInvariant.SystemConfig.LoginProvider;
                        if (loginProvider == "Api")
                        {
                            context.Token = context.Request.Headers[GlobalInvariant.SystemConfig.JwtSetting.TokenName].ParseToString();
                        }
                        else
                        {
                            context.Token = context.Request.Cookies[GlobalInvariant.SystemConfig.JwtSetting.TokenName].ParseToString();
                        }

                        return Task.CompletedTask;
                    }
                };
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = GlobalInvariant.SystemConfig.JwtSetting.Issuer,
                    ValidAudience = GlobalInvariant.SystemConfig.JwtSetting.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalInvariant.SystemConfig.JwtSetting.TokenKey))
                };
            });
        }

        /// <summary>
        /// ����ע��Autofac
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
                option.SwaggerEndpoint("/swagger/V1/swagger.json", "MyBlog WebApi");
            });

            app.UseRouting();

            //���� ������UseRouting֮��,UseAuthorization֮ǰ
            app.UseCors("AllowCors");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            GlobalInvariant.ServiceProvider = app.ApplicationServices;
        }
    }
}
