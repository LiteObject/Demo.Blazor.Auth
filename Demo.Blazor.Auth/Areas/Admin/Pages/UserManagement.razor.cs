using Demo.Blazor.Auth.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Blazor.Auth.Areas.Admin.Pages
{
    [Authorize(Roles ="Admin")]
    public partial class UserManagement
    {
        [Inject]
        protected UserManager<ApplicationUser> UserManager { get; set; }

        private List<ApplicationUser> users = new();

        private ApplicationUser selectedApplicationUser;

        public UserManagement() { }

        protected override async Task OnInitializedAsync()
        {
            users = await UserManager.Users.ToListAsync();
        }

        /*protected async Task AddUser(ApplicationUser newApplicationUser)
        {
            editContext = new(newApplicationUser);

            if (editContext.Validate())
            {
            }
        }*/

        protected void SetSelectedApplicationUser(ApplicationUser user) 
        {
            this.selectedApplicationUser = user;
            Console.WriteLine($"You have selected: {this.selectedApplicationUser.Email}");
        }

        private async Task OnChildUpdate(string newMessage)
        {
            Console.WriteLine($">>>>> Parent: {newMessage}");
            users = await UserManager.Users.ToListAsync();
        }
    }
}
