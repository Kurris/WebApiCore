using Autofac;
using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Utils;
using WebApiCore.Utils.Extensions;
using WebApiCore.Utils.Model;
using System;

namespace Ligy.Project.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();

            services.AddSwaggerGen(setupAction =>
               {
                   setupAction.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo()
                   {
                       Version = "Ver 1",
                       Title = "WebApi",
                   });
               });

            //自定义特性
            services.AddMvc(option =>
            {
                option.Filters.Add<CustomExceptionFilterAttribute>();
                option.Filters.Add<CustomResourceFilterAttribute>();
                option.Filters.Add<CustomActionAndResultFilterAttribute>();
            });

            services.AddCors(option =>
            {
                option.AddPolicy("AllowCors", builder =>
               {
                   builder.AllowAnyOrigin().AllowAnyMethod();
               });
            });
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddSession();

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
                        if (loginProvider == "WebApi")
                            context.Token = context.Request.Headers[GlobalInvariant.SystemConfig.JwtSetting.TokenName].ParseToString();
                        else if (loginProvider == "Session")
                            context.Token = context.HttpContext.Session.GetString(GlobalInvariant.SystemConfig.JwtSetting.TokenName);
                        else if (loginProvider == "Cookie")
                            context.Token = context.Request.Cookies[GlobalInvariant.SystemConfig.JwtSetting.TokenName].ParseToString();
                        else
                            throw new NotSupportedException(loginProvider);

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
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
        /// 依赖注入Autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder) => builder.RegisterModule<CustomAutofacModule>();


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/V1/swagger.json", "WebApi");
            });
            app.UseRouting();
            //跨域 必须在UseRouting之后,UseAuthorization之前
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
