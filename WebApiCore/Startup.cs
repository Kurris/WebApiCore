using Autofac;
using Ligy.Project.WebApi.CustomClass;
using WebApiCore.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using WebApiCore.Utils.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(op =>
                {
                    op.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["access_token"];
                            return Task.CompletedTask;
                        }
                    };

                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "ligy.site",
                        ValidAudience = "api",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678987654321"))
                    };
                });

            GlobalInvariant.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
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
        }
    }
}
