using System;
using Microsoft.AspNetCore.Identity;

namespace Demo.Blazor.Auth.Areas.Identity.Data
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string Organization { get; set; }
        
        [PersonalData]
        public string JobTitle { get; set; }

        [PersonalData]
        public DateTime? CreatedOn { get; set; }

        [PersonalData]
        public string CreatedBy { get; set; } = "System";

        [PersonalData]
        public DateTime? UpdatedOn { get; set; }

        [PersonalData]
        public string UpdatedBy { get; set; } = "System";
    }
}
