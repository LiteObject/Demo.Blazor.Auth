using Blazored.Toast.Services;
using Demo.Blazor.Auth.Areas.Identity.Data;
using Demo.Blazor.Auth.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Blazor.Auth.Areas.Admin.Components
{
    public partial class AddOrEditUser
    {
        public enum ActionType {
            [Description("Add User")]
            Add = 0,
            [Description("Edit User")]
            Edit = 1
        }

        [Parameter] 
        public EventCallback<string> OnUpdate { get; set; }

        [Parameter]
        public ActionType ComponentAction { get; set; } = ActionType.Add;

        [Parameter]
        public ApplicationUser SelectedApplicationUser { get; set; } = new();

        [Inject]
        private UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        private RoleManager<IdentityRole> RoleManager { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        public InputModel InputUserModel { get; set;  }

        private EditContext editContext;

        private List<IdentityRole> availableRoles = new();

        private List<string> userRoles = new();

        protected override void OnParametersSet()
        {
            // user = GetUser(Id);
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            availableRoles = await RoleManager.Roles.ToListAsync();

            if(ComponentAction == ActionType.Add)
            {
                InputUserModel = new InputModel();
                userRoles = new();
            }
            else if(ComponentAction == ActionType.Edit)
            {
                InputUserModel = GetInputModel(SelectedApplicationUser);
                userRoles = await GetUserRolesAsync(SelectedApplicationUser);
            }            

            editContext = new(InputUserModel);
            
        }

        private async Task HandleSubmit()
        {
            if (editContext.IsModified()) 
            {
                Console.WriteLine($"editContext.IsModified(): {editContext.IsModified()}");
            }

            if (editContext.Validate())
            {
                Console.WriteLine("HandleSubmit called: Form is valid");

                var user = await UserManager.FindByIdAsync(InputUserModel.Id) ?? new ApplicationUser();

                user.FirstName = InputUserModel.FirstName;
                user.LastName = InputUserModel.LastName;
                user.PhoneNumber = InputUserModel.PhoneNumber;
                user.Organization = InputUserModel.PhoneNumber;
                user.JobTitle = InputUserModel.JobTitle;

                IdentityResult result = new();

                switch (ComponentAction) 
                {
                    case ActionType.Edit:
                        user.UpdatedOn = DateTime.UtcNow;
                        // https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-5.0
                        user.UpdatedBy = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.Name;
                        result = await UserManager.UpdateAsync(user);
                        break;
                    case ActionType.Add:
                        user.UserName = InputUserModel.Email;
                        user.Email = InputUserModel.Email;
                        user.EmailConfirmed = true;
                        user.PhoneNumberConfirmed = true;
                        user.CreatedOn = DateTime.UtcNow;                        
                        // https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-5.0
                        user.CreatedBy = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.Name;
                        result = await UserManager.CreateAsync(user, InputUserModel.Password);                        
                        break;
                }
                
                if (result.Succeeded)
                {
                    SelectedApplicationUser = user;
                    var successMessage = $"Successully performed {ComponentAction.GetDescription().ToLower()} ({InputUserModel.Email}) action.";                    
                    ToastService.ShowSuccess(successMessage);
                    this.InputUserModel = GetInputModel(user);
                    await this.OnUpdate.InvokeAsync(successMessage);
                    Console.WriteLine(successMessage);
                }
                else 
                {
                    StringBuilder errorMessage = new StringBuilder();

                    foreach (var e in result.Errors) 
                    {
                        errorMessage.AppendLine($"Error while performing {ComponentAction.GetDescription().ToLower()} action. {e.Code}: {e.Description}");
                    }

                    ToastService.ShowError(errorMessage.ToString());                    
                    await this.OnUpdate.InvokeAsync(errorMessage.ToString());
                    Console.WriteLine(errorMessage.ToString());
                }               
            }
            else
            {
                Console.WriteLine("HandleSubmit called: Form is INVALID");
            }
        }

        private InputModel GetInputModel(ApplicationUser user) 
        {
            if (string.IsNullOrWhiteSpace(SelectedApplicationUser?.Email))
            {
                Console.WriteLine($"Parameter \"{nameof(user)}\" passed to method \"{nameof(GetInputModel)}\" is null or empty.");        
            }

            return new InputModel
            {
                Id = SelectedApplicationUser.Id,
                Email = SelectedApplicationUser.Email,
                FirstName = SelectedApplicationUser.FirstName,
                LastName = SelectedApplicationUser.LastName,
                Organization = SelectedApplicationUser.Organization,
                JobTitle = SelectedApplicationUser.JobTitle,
                PhoneNumber = SelectedApplicationUser.PhoneNumber,
                UpdatedOn = SelectedApplicationUser.UpdatedOn?.ToLocalTime(),
                UpdatedBy = SelectedApplicationUser.UpdatedBy
            };
        }

        private async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return new List<string>(await UserManager.GetRolesAsync(user));
        }

        private async Task UpdateUserRoles(IdentityRole role, ChangeEventArgs e) 
        {
            var val = e.Value;

            if ((bool)val)
            {
                // Add role
                Console.WriteLine($"Adding role \"{role.Name}\"");
                var result = await this.UserManager.AddToRoleAsync(SelectedApplicationUser, role.Name);

                if (result.Succeeded)
                {
                    Console.WriteLine($"Successfully added user \"{SelectedApplicationUser.UserName}\" to role \"{role.Name}\"");
                }
                else
                {
                    Console.WriteLine($"Failed to add user \"{SelectedApplicationUser.UserName}\" to role \"{role.Name}\"");
                }
            }
            else {
                // remove role
                Console.WriteLine($"Removing role \"{role.Name}\"");
                var result = await this.UserManager.RemoveFromRoleAsync(SelectedApplicationUser, role.Name);

                if (result.Succeeded)
                {
                    Console.WriteLine($"Successfully removed user \"{SelectedApplicationUser.UserName}\" from role \"{role.Name}\"");
                }
                else {
                    Console.WriteLine($"Failed to remove user \"{SelectedApplicationUser.UserName}\" from role \"{role.Name}\"");
                }
            }
        }

        public class InputModel
        {

            public string Id { get; set; }

            [Display(Name = "First Name")]
            [Required]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [Required]
            public string LastName { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            [Required]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Organization")]
            [StringLength(50, ErrorMessage = "Organization name too long (50 character limit).")]
            public string Organization { get; set; }

            [Required]
            [Display(Name = "Job Title")]
            [StringLength(50, ErrorMessage = "Job title too long (50 character limit).")]
            public string JobTitle { get; set; }

            [Display(Name = "Email Address")]
            public string Email { get; set; }

            // [PasswordPropertyText]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Updated On")]
            public DateTime? UpdatedOn { get; set; }

            [Display(Name = "Updated By")]
            public string UpdatedBy { get; set; }
        }
    }
}
