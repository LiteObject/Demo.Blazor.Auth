using Demo.Blazor.Auth.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Blazor.Auth.Data
{
    public static class InitialSeedData
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToList();

            if (!roles.Any(r => r.Name == Models.Enums.Roles.Admin.ToString())) {                
                await roleManager.CreateAsync(new IdentityRole(Models.Enums.Roles.Admin.ToString()));                
            }

            if (!roles.Any(r => r.Name == Models.Enums.Roles.Basic.ToString())) {
                await roleManager.CreateAsync(new IdentityRole(Models.Enums.Roles.Basic.ToString()));
            }
        }
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default Admin User
            var defaultAdminUser = new ApplicationUser
            {
                UserName = "tuniis@live.com",
                Email = "tuniis@live.com",
                FirstName = "Mohammed",
                LastName = "Hoque",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Organization = "SPARSOft",
                JobTitle = "Software Enginner"
            };

            if (userManager.Users.All(u => u.UserName != defaultAdminUser.UserName))
            {
                var user = await userManager.FindByEmailAsync(defaultAdminUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdminUser, "Demo.01");
                    await userManager.AddToRoleAsync(defaultAdminUser, Models.Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultAdminUser, Models.Enums.Roles.Admin.ToString());
                }
            }

            var defaultUser = new ApplicationUser
            {
                UserName = "Lite.Object@gmail.com",
                Email = "Lite.Object@gmail.com",
                FirstName = "MX",
                LastName = "NET",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Organization = "LiteObject",
                JobTitle = "Researcher"
            };

            if (userManager.Users.All(u => u.UserName != defaultAdminUser.UserName))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Demo.02");
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Basic.ToString());
                }
            }
        }
    }
}
