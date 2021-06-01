using Demo.Blazor.Auth.Areas.Identity.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Blazor.Auth.Areas.Admin.Pages
{
    public partial class RoleManagement
    {
        [Inject]
        private RoleManager<IdentityRole> RoleManager { get; set; }

        [Inject]
        private UserManager<ApplicationUser> UserManager { get; set; }

        private List<IdentityRole> roles = new();

        public RoleManagement() { }

        protected override async Task OnInitializedAsync() 
        {
            roles = await RoleManager.Roles.ToListAsync();
        }

        private async Task AddRole(string roleName) 
        {
            if (!string.IsNullOrWhiteSpace(roleName)) 
            {
                var result = await RoleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded) 
                {
                    Console.WriteLine($" Successfully created \"{roleName}\" role.");
                }                
            }
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await UserManager.GetRolesAsync(user));
        }
    }
}
