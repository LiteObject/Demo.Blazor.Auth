using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Blazor.Auth.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Demo.Blazor.Auth.Areas.Identity.IdentityHostingStartup))]
namespace Demo.Blazor.Auth.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>(options => {
                    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Demo.Blazor.Auth;Trusted_Connection=True;MultipleActiveResultSets=true");
                });

                // services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();

                // For more info: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0
                services.AddDefaultIdentity<ApplicationUser>(
                        options =>
                        {
                            // options.SignIn.RequireConfirmedAccount = true;

                            // User settings.
                            options.User.AllowedUserNameCharacters =
                                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                            options.User.RequireUniqueEmail = true;

                            // Password settings
                            options.Password.RequireDigit = true;
                            options.Password.RequireLowercase = true;
                            options.Password.RequireNonAlphanumeric = true;
                            options.Password.RequireUppercase = true;
                            options.Password.RequiredLength = 6;
                            options.Password.RequiredUniqueChars = 1;

                            // Lockout settings.
                            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                            options.Lockout.MaxFailedAccessAttempts = 5;
                            options.Lockout.AllowedForNewUsers = true;
                        })
                    .AddEntityFrameworkStores<AppDbContext>();

                services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            });
        }
    }
}
