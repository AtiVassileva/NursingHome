using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Common;
using NursingHome.DAL.Models;

namespace NursingHome.UI.Infrastructure
{
    using static Common.WebConstants;
    using static ModelConstants;

    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;

            MigrateDatabase(serviceProvider);
            await SeedRolesAsync(serviceProvider);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            string[] roles = { RegularUserRoleName, EmployeeRoleName, AdministratorRoleName };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }
            }
            
            var existingAdmin = await userManager.FindByEmailAsync("admin1@abv.bg");

            if (existingAdmin == null)
            {
                var admin = new ApplicationUser
                {
                    Email = "admin1@abv.bg",
                    UserName = "admin",
                    FirstName = "Георги",
                    MiddleName = "Иванов",
                    LastName = "Стаменов",
                    UserStatus = UserStatus.Active,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "admin123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AdministratorRoleName);
                }
            }
        }
    }
}