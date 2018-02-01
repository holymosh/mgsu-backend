using System;
using System.Threading.Tasks;
using MGSUCore.Authentification;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Common.Entities;
using Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpOverrides;

namespace MGSUCore
{
    public class Startup
    {
        private readonly DeploySettings _deploySettings;

        public Startup(IHostingEnvironment env, IOptions<DeploySettings> deploySettings)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                //.AddJsonFile("appsettings.json", optional: fale, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _deploySettings = deploySettings.Value;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.  
            //services.AddApplicationInsightsTelemetry(Configuration);  

            // Add framework services.
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsInRole(UserRole.Admin)));
                options.AddPolicy(
                    "User",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsAuthentificated()));
            });

            services.AddAuthentication("WebCookieAuthMiddleware")
                .AddCookie("WebCookieAuthMiddleware", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.FromResult(0);
                    };
                    options.Cookie.SameSite = SameSiteMode.None;
                })
                ;

            //Add DI starter
            new Bootstraper(services, Configuration).Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            ////todo: use cookie auth middleware
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationScheme = "WebCookieAuthMiddleware",
            //    //todo: remove magic 302 bug
            //    LoginPath = PathString.Empty,
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true
            //});
            app.UseAuthentication();
            //CORS
            app.UseCors(builder => builder.WithOrigins("http://efund-mgsu.ru",
                "http://185.204.0.35:5001",
                "http://localhost:3000").AllowAnyMethod().AllowCredentials().AllowAnyHeader());
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            app.UseMvc();
        }
    }
}