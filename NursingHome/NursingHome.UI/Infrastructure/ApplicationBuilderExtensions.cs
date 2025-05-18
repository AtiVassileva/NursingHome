using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NursingHome.BLL;
using NursingHome.DAL;
using NursingHome.DAL.Common;
using NursingHome.DAL.Models;
using NursingHome.UI.PdfGenerators;
using NursingHome.UI.Services;

namespace NursingHome.UI.Infrastructure
{
    using static Common.WebConstants;
    using static ModelConstants;

    public static class ApplicationBuilderExtensions
    {
        public static void RegisterServicesCollection(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddTransient<UserService>()
                .AddTransient<UserUiService>()
                .AddTransient<ResidentInfoService>()
                .AddTransient<EmployeeInfoService>()
                .AddTransient<MonthlyParameterService>()
                .AddTransient<MonthlyFeeService>()
                .AddTransient<FeeBookPdfGenerator>()
                .AddTransient<FeeBookUiService>()
                .AddTransient<PaymentService>()
                .AddTransient<MedicalRecordService>()
                .AddTransient<SocialDocumentService>()
                .AddTransient<ReportService>()
                .AddTransient<WeeklyMenuService>()
                .AddTransient<FileUiService>()
                .AddScoped<IClaimsTransformation, PositionClaimsTransformer>();
        }

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
                .GetRequiredService<NursingHomeDbContext>();
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

            const string adminEmail = "admin@abv.bg";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var admin = new ApplicationUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "Георги",
                    MiddleName = "Иванов",
                    LastName = "Стаменов",
                    UserStatus = UserStatus.Active
                };

                const string adminPassword = "admin123";

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AdministratorRoleName);
                }
            }
        }
    }
}