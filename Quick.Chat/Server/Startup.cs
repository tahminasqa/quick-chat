using Quick.Chat.Server.Data;
using Quick.Chat.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.Configuration;
using Serilog;
using Microsoft.Extensions.Logging;
using Quick.Chat.Server.Areas.Identity;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR;

namespace Quick.Chat.Server
{
    public class Startup
    {
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            //Create a logger factory
            var loggerFactory = new LoggerFactory().AddSerilog(logger);

            //Get a logger
            var startupLogger = loggerFactory.CreateLogger<Startup>();



            StringBuilder sb = new();
            sb.AppendLine();
            sb.AppendLine("******************************************************");
            sb.AppendLine("**                  QuickChat                      **");
            sb.AppendLine("**              [Version 1.1.0.0]                  **");
            sb.AppendLine("**     ©2024 QuickChat. All rights reserved        **");
            sb.AppendLine("******************************************************");

            startupLogger.LogInformation(sb.ToString());

            services.AddSignalR();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("QuickChatServerDB")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();



            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //       .AddCookie(options =>
            //       {
            //           // add an instance of the patched manager to the options:
            //           options.CookieManager = new ChunkingCookieManager();

            //           options.Cookie.HttpOnly = true;
            //           options.Cookie.SameSite = SameSiteMode.None;
            //           options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            //       })
            services.AddAuthentication()
               .AddIdentityServerJwt();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.OnAppendCookie = cookieContext =>
                    AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
            /*
            services.AddDataProtection()
                    .SetApplicationName("Quick.Chat.Server")
                    .PersistKeysToFileSystem(new DirectoryInfo(string.Format(@"{0}opt{0}app{0}sysfiles{0}", Path.DirectorySeparatorChar)))
                    .ProtectKeysWithCertificate(new X509Certificate2(string.Format(@"{0}certificates{0}verify-cert.pfx", Path.DirectorySeparatorChar), Configuration["AppSettings:CertificatePassword"]))
                    .SetDefaultKeyLifetime(TimeSpan.FromDays(60));

           */
            services.Configure<CookieTempDataProviderOptions>(config =>
            {
                config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHub<SignalRHub>("/signalRHub");
                // SignalR endpoint routing setup
                endpoints.MapHub<Hubs.ChatHub>(ChatClient.HUBURL);
            });
        }
    }
}
