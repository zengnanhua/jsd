using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdminSystem.Api.Infrastructure.AutofacModules;
using AdminSystem.Api.Infrastructure.Filters;
using AdminSystem.Application.Services;
using AdminSystem.Infrastructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.MySql;
using Hangfire.MySql.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AdminSystem.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

      
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {


            services.AddMemoryCache();

            services
              .AddSystemRegisterType(Configuration)
              .AddCustomSwagger(Configuration)
              .AddCustomDbContext(Configuration)
              .AddAuthorization()
              .AddMvc(options =>
              {
                  options.Filters.Add(typeof(HttpGlobalExceptionFilter));
              })
              .AddControllersAsServices();


            //services.AddMvc()

            #region 验证权限
            // services.AddAuthentication("Bearer")
            //.AddIdentityServerAuthentication(options =>
            //{
            //    options.Authority = "http://localhost:57988";
            //    options.RequireHttpsMetadata = false;
            //    options.ApiName = "admin_api";
            //});


           
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(o => {
                 //主要是jwt  token参数设置
                 o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                 {
                     //Token颁发机构
                     ValidIssuer = "self",
                     //颁发给谁
                     ValidAudience = "selfA",
                     //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hello-key-----wyt"))
                     //ValidateIssuerSigningKey=true,
                     ////是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                     //ValidateLifetime=true,
                     ////允许的服务器时间偏移量
                     //ClockSkew=TimeSpan.Zero

                 };
             });
         

            #endregion

            #region 使用 autofac
            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule(Configuration.GetConnectionString("MysqlConnection")));

            #endregion

            return new AutofacServiceProvider(container.Build());
        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseHangfireServer();//启动Hangfire服务
            app.UseHangfireDashboard();//启动hangfire面板


            app.UseAuthentication();

            app.UseMvc(routes=> {
                routes.MapRoute(
                    name: "Default",
                    template: "api/{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"/swagger/v1/swagger.json", "adminSystem.Api");
                   c.DocExpansion(DocExpansion.None);
                   //c.OAuthClientId("orderingswaggerui");
                   //c.OAuthAppName("Ordering Swagger UI");
               });
        }
    }
    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddSystemRegisterType(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();
            return services;
        }

        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHangfire(x => x.UseStorage(new MySqlStorage(configuration.GetConnectionString("MysqlConnection"), new MySqlStorageOptions() { TablePrefix = "h_" })));

            services.AddEntityFrameworkMySql()
                   .AddDbContext<ApplicationDbContext>(options =>
                   {
                       options.UseMySql(configuration.GetConnectionString("MysqlConnection"),
                           mySqlOptionsAction: sqlOptions =>
                           {
                               //sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                               sqlOptions.MigrationsAssembly("AdminSystem.Api");
                               sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                           });
                   },
                       ServiceLifetime.Scoped
                    );




            //services.AddDbContext<IntegrationEventLogContext>(options =>
            //{
            //    options.UseSqlServer(configuration["ConnectionString"],
            //                         sqlServerOptionsAction: sqlOptions =>
            //                         {
            //                             sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            //                             //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
            //                             sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            //                         });
            //});

            return services;
        }

        
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
           

            services.AddSwaggerGen(options =>
            {
                //options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "adminSystem.Api",
                    Version = "v1",
                    Description = "The adminSystem Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });

                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "AdminSystem.Api.xml"));

                //options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    Flow = "implicit",
                //    AuthorizationUrl = $"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize",
                //    TokenUrl = $"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token",
                //    Scopes = new Dictionary<string, string>()
                //    {
                //        { "orders", "Ordering API" }
                //    }
                //});

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }
    }
}
