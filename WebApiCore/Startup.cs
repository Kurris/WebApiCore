using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
                   setupAction.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo()
                   {
                       Version = "Ver 1",
                       Title = "WebApi",
                   });
               });

            //�Զ�������
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
                            OnChallenge = context =>
                            {
                                if (!context.HttpContext.User.Identity.IsAuthenticated)
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                                    context.Response.ContentType = "application/json";

                                    string result = JsonHelper.ToJson(new TData<string>()
                                    {
                                        Data = null,
                                        Message = "��Ȩʧ��",
                                        Status = Status.AuthorizationFail
                                    });
                                    byte[] content = Encoding.UTF8.GetBytes(result);
                                    context.Response.ContentLength = content.Length;
                                    context.Response.BodyWriter.WriteAsync(new ReadOnlyMemory<byte>(content));
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
        public void ConfigureContainer(ContainerBuilder builder) => builder.RegisterModule<AutofacModule>();


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
            app.UseCors(_corsPolicy);
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
