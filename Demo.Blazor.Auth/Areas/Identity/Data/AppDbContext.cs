using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Demo.Blazor.Auth.Areas.Identity.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasKey(e => e.Id);

            /* https://docs.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations
             * modelBuilder.Entity<ApplicationUser>()
                .Property(b => b.UpdatedOn)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("GETUTCDATE()") */
            // .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<ApplicationUser>()
              .Property(b => b.CreatedOn)
              .ValueGeneratedOnAdd()
              .HasComputedColumnSql("GETUTCDATE()");

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        public override void Dispose()
        {
            Console.WriteLine($"{ContextId} context disposed.");
            base.Dispose();
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/></returns>
        public override ValueTask DisposeAsync()
        {
            Console.WriteLine($"{ContextId} context disposed async.");
            return base.DisposeAsync();
        }
    }
}
