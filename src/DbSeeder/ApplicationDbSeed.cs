using Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DbSeeder
{
    public static class ApplicationDbSeed
    {
        public static async Task SeedAdminRoleAsync(ILogger logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRole = new IdentityRole("Administrator");
            if (!roleManager.Roles.Any(r => r.Name == adminRole.Name))
            {
                logger.LogInformation("Administrator role note found. Creating Adminsitrator role.");
                await roleManager.CreateAsync(adminRole);
                await SeedAdminClaims(logger, adminRole, roleManager);
                await SeedAdminAccount(logger, userManager);
            }            
        }

        public static async Task SeedAdminClaims(ILogger logger, IdentityRole role, RoleManager<IdentityRole> roleManager)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            if (!claims.Any(c => c.Value == "CanPurge"))
            {
                logger.LogInformation("Adding CanPurge Permission claim.");
                await roleManager.AddClaimAsync(role, new Claim("Permission", "CanPurge"));
            }
        }

        public static async Task SeedAdminAccount(ILogger logger, UserManager<ApplicationUser> userManager)
        {
            var adminAccount = new ApplicationUser { UserName = "admin@test.com", Email = "admin@test.com", EmailConfirmed = true };

            if (!userManager.Users.Any(u => u.UserName == adminAccount.UserName))
            {
                logger.LogInformation("Creating administrator user account.");
                await userManager.CreateAsync(adminAccount, "Pass123!!");
                await userManager.AddToRoleAsync(adminAccount, "Administrator");
            }
        }
    }
}
