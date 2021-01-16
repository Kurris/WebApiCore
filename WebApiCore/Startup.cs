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
using Newtonsoft.Json;
using WebApiCore.CustomClass;
using WebApiCore.Data.EF;
using WebApiCore.Hubs;
using WebApiCore.Lib.Utils;
using WebApiCore.Lib.Utils.Extensions;
using WebApiCore.Lib.Utils.Config;

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
            services.AddHttpClient();
            services.AddControllers().AddControllersAsServices().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen(option =>
               {
                   option.SwaggerDoc("v1", new OpenApiInfo()
                   {
                       Version = "Ver 1",
                       Title = "WebApi",
                   });
               });
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
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(GlobalInvariant.SystemConfig.JwtConfig.Expiration);
                option.Cookie.Name = GlobalInvariant.SystemConfig.JwtConfig.TokenName;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(op =>
                    {
                        op.Events = new JwtBearerEvents()
                        {
                            OnMessageReceived = context =>
                            {
                                string loginProvider = GlobalInvariant.SystemConfig.LoginProvider;
                                string tokenName = GlobalInvariant.SystemConfig.JwtConfig.TokenName;

                                if (!context.HttpContext.Request.Path.StartsWithSegments("/api")) loginProvider = "Signalr";

                                context.Token = loginProvider switch
                                {
                                    "WebApi" => context.Request.Headers[tokenName].ParseToStr(),
                                    "Session" => context.Token = context.HttpContext.Session.GetString(tokenName),
                                    "Cookie" => context.Request.Cookies[tokenName].ParseToStr(),
                                    "Signalr" => context.Request.Query["access_token"],
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
                            ValidIssuer = GlobalInvariant.SystemConfig.JwtConfig.Issuer,
                            ValidAudience = GlobalInvariant.SystemConfig.JwtConfig.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalInvariant.SystemConfig.JwtConfig.TokenSecretKey)),
                            RequireExpirationTime = true,
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

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
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
                endpoints.MapHub<ChatHub>("/chat");
            });

            GlobalInvariant.ServiceProvider = app.ApplicationServices;

            var dbContext = EFDB.Instance.DbContext;
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
            dbContext.Dispose();


            if (!GlobalInvariant.SystemConfig.IsDebug)
            {
                //lifetime.ApplicationStarted.Register(async () =>
                //{
                //    var autoJob = GlobalInvariant.ServiceProvider.GetService<IJobCenter>();
                //    await autoJob.Start();
                //});
                //lifetime.ApplicationStopping.Register(async () =>
                //{
                //    var autoJob = GlobalInvariant.ServiceProvider.GetService<IJobCenter>();
                //    await autoJob.StopAll();
                //});
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("|StartUp:  " + DateTime.Now);
            sb.AppendLine("|LoginProvider:  " + GlobalInvariant.SystemConfig.LoginProvider);
            sb.AppendLine("|CacheProvider:  " + GlobalInvariant.SystemConfig.CacheProvider);
            sb.AppendLine("|Machine:  " + Environment.MachineName + "  " + Environment.OSVersion.Platform + "  " + Environment.OSVersion.VersionString);
            sb.AppendLine("|ContentRootPath:  " + env.ContentRootPath);
            sb.AppendLine("|IsDevelopment:  " + env.IsDevelopment());
            sb.AppendLine("|Version:  " + GlobalInvariant.Version);
            logger.LogInformation(sb.ToString());
        }
    }
}
