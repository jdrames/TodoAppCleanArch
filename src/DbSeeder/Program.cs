using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbSeeder
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider;
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    if (context.Database.IsSqlServer())
                    {
                        logger.LogInformation("Attempting to apply migrations.");                        
                        context.Database.Migrate();
                    }

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await ApplicationDbSeed.SeedAdminRoleAsync(logger, userManager, roleManager);
                }
                catch(Exception ex)
                {                    
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }

            //await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {                    
                    services.AddApplicationLayer();
                    services.AddInfrastructureLayer(hostContext.Configuration);
                    services.AddSingleton<ICurrentUserService, CurrentUserService>();
                    services.AddAuthentication();
                    services.AddAuthorization(options =>
                    {
                        var policyBuilder = new AuthorizationPolicyBuilder(IdentityConstants.ApplicationScheme);
                        policyBuilder.RequireAuthenticatedUser();
                        options.DefaultPolicy = policyBuilder.Build();

                        // Custom policies based on users roles
                        options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
                    });
                });
    }
}
