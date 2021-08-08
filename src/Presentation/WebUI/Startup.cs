using Application.Common.Extensions;
using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedServices;
using System.Reflection;

namespace WebUI
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
            services.AddApplicationLayer();
            services.AddInfrastructureLayer(Configuration);
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IEmailSender, EmailSenderService>();

            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                var policyBuilder = new AuthorizationPolicyBuilder(IdentityConstants.ApplicationScheme);
                policyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = policyBuilder.Build();

                // Custom policies based on users roles
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

            // Bundle Service for bundled and minified CSS / JS files
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddScssBundle("/css/bootstrap/bootstrap.css", "lib/cdnjs/bootstrap/scss/custom.scss");
                pipeline.AddJavaScriptBundle("/js/bootstrap/bootstrap.js", "lib/cdnjs/bootstrap/js/bootstrap.js");
            });

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Home/Index", "");
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Login");
                })
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseWebOptimizer();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
