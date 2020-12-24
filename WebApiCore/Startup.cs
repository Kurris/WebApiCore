using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiCore.AutoJobInterface;
using WebApiCore.CustomClass;
using WebApiCore.EF;
using WebApiCore.Hubs;
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

            services.AddSignalR();
            services.AddControllers().AddControllersAsServices()
                    .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
            services.AddSwaggerGen(option =>
               {
                   option.SwaggerDoc("V1", new OpenApiInfo()
                   {
                       Version = "Ver 1",
                       Title = "WebApi",
                   });
               }).AddAuthentication();
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
            services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(GlobalInvariant.SystemConfig.JwtSetting.Expiration));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(op =>
                    {
                        op.Events = new JwtBearerEvents()
                        {
                            OnMessageReceived = context =>
                            {
                                string loginProvider = GlobalInvariant.SystemConfig.LoginProvider;
                                string tokenName = GlobalInvariant.SystemConfig.JwtSetting.TokenName;
                                context.Token = loginProvider switch
                                {
                                    "WebApi" => context.Request.Headers[tokenName].ParseToString(),
                                    "Session" => context.Token = context.HttpContext.Session.GetString(tokenName),
                                    "Cookie" => context.Request.Cookies[tokenName].ParseToString(),
                                    _ => throw new NotSupportedException(loginProvider)
                                };
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
        /// “¿¿µ◊¢»ÎAutofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder) => builder.RegisterModule<AutofacModule>();


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                GlobalInvariant.SystemConfig.IsDebug = true;
                app.UseDeveloperExceptionPage();
            }

            //app.UseWebSockets(new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(120),
            //    ReceiveBufferSize = 4 * 1024
            //});
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/V1/swagger.json", "WebApi");
            });
            app.UseSession();
            app.UseRouting();
            app.UseCors(_corsPolicy);
            app.UseMiddleware<GlobalMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("api/chat");
            });

            GlobalInvariant.ServiceProvider = app.ApplicationServices;

            var op = EFDB.Create().GetIDataBaseOperation();
            if (op.DbContext.Database.GetPendingMigrations().Any())
            {
                op.DbContext.Database.Migrate();
            }

            lifetime.ApplicationStarted.Register(async () =>
            {
                var autoJob = GlobalInvariant.ServiceProvider.GetService<IJobCenter>();
                await autoJob.Start();
            });
            lifetime.ApplicationStopping.Register(async () =>
            {
                var autoJob = GlobalInvariant.ServiceProvider.GetService<IJobCenter>();
                await autoJob.StopAll();
            });

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("|≥Ã–Ú∆Ù∂Ø");
            sb.AppendLine("|ContentRootPath:" + env.ContentRootPath);
            sb.AppendLine("|WebRootPath:" + env.WebRootPath);
            sb.AppendLine("|IsDevelopment:" + env.IsDevelopment());
            sb.AppendLine("|Version:" + GlobalInvariant.Version);
            logger.LogInformation(sb.ToString());
        }
    }
}
