using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using FeezSpeedy.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FeezSpeedy.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Parent>>();

            // 🔹 Ensure DB
            await context.Database.MigrateAsync();

            // 🔹 Roles
            string[] roles = { "Admin", "Parent" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 🔹 Admin User
            const string adminEmail = "admin@feezspeedy.co.ke";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new Parent
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NationalId = "00000000",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@1234");
                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Ensure Admin Role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // 🔹 Payment Options
            if (!context.PaymentOptions.Any())
            {
                context.PaymentOptions.AddRange(
                    new PaymentOption { Name = "MPESA", ProviderCode = "MPESA", IsActive = true },
                    new PaymentOption { Name = "Bank Transfer", ProviderCode = "BANK", IsActive = true }
                );
            }

            // 🔹 Loan Statuses (example – adjust names to your enums)
            // 🔹 Loan Statuses
            if (!context.LoanStatuses.Any())
            {
                context.LoanStatuses.AddRange(
                    new LoanStatus { Name = "Pending", SortOrder = 1 },
                    new LoanStatus { Name = "Approved", SortOrder = 2 },
                    new LoanStatus { Name = "Disbursed", SortOrder = 3 },
                    new LoanStatus { Name = "Completed", SortOrder = 4 },
                    new LoanStatus { Name = "Declined", SortOrder = 5 }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}