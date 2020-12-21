using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Text;
using WebApiCore.AutoJob;
using WebApiCore.CustomClass;
using WebApiCore.Utils;
using WebApiCore.Utils.Extensions;
using WebApiCore.Utils.Model;

namespace WebApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly string _corsPolicy = "AllowCors";

        public void ConfigureServices(IServiceCollection services)
        {
            GlobalInvariant.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalInvariant.Configuration = Configuration;

            services.AddControllers().AddControllersAsServices()
                    .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
            services.AddSwaggerGen(setupAction =>
               {
                   setupAction.SwaggerDoc("V1", new OpenApiInfo()
                   {
                       Version = "Ver 1",
                       Title = "WebApi",
                   });
               });
            //自定义特性
            services.AddMvc(option =>
            {
                option.Filters.AddService<CustomExceptionFilterAttribute>();
                option.Filters.AddService<CustomResourceFilterAttribute>();
                option.Filters.AddService<CustomActionAndResultFilterAttribute>();
            });
            services.AddCors(op =>
            {
                op.AddPolicy(_corsPolicy, builder =>
                 {
                     builder.WithOrigins(GlobalInvariant.SystemConfig.AllowHosts.Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                 });
            });
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(op =>
                    {
                        op.Events = new JwtBearerEvents()
                        {
                            OnMessageReceived = async context =>
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
                             },
                            OnChallenge = async context =>
                            {
                                if (!context.HttpContext.User.Identity.IsAuthenticated)
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                                    context.Response.ContentType = "application/json";

                                    string result = JsonHelper.ToJson(new TData<string>()
                                    {
                                        Data = null,
                                        Message = "授权失败",
                                        Status = Status.AuthorizationFail
                                    });
                                    byte[] content = Encoding.UTF8.GetBytes(result);
                                    context.Response.ContentLength = content.Length;
                                    await context.Response.BodyWriter.WriteAsync(new ReadOnlyMemory<byte>(content));
                                }
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
        public void ConfigureContainer(ContainerBuilder builder) => builder.RegisterModule<AutofacModule>();


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                GlobalInvariant.SystemConfig.IsDebug = true;
                //app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/V1/swagger.json", "WebApi");
            });
            app.UseRouting();
            app.UseCors(_corsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            lifetime.ApplicationStarted.Register(async () =>
            {
                var autoJob = GlobalInvariant.ServiceProvider.GetService<IJobCenter>();
                await autoJob.Start();
            });
            lifetime.ApplicationStopping.Register(async () =>
            {
                var autoJob = GlobalInvariant.ServiceProvider.GetService<IJobCenter>();
                await autoJob.Stop();
            });

            GlobalInvariant.ServiceProvider = app.ApplicationServices;
        }
    }
}
