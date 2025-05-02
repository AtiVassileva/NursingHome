using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NursingHome.DAL.Models;

namespace NursingHome.UI.Infrastructure
{
    using static Common.WebConstants;
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);

        public static bool IsEmployee(this ClaimsPrincipal user)
            => user.IsInRole(EmployeeRoleName);

        public static bool IsResident(this ClaimsPrincipal user)
            => user.IsInRole(RegularUserRoleName);

        public static string GetFullName(this ClaimsPrincipal user, UserManager<ApplicationUser> userManager)
        {
            var fullName = string.Empty;

            Task.Run(async () =>
                {
                    var userDetails = await userManager.GetUserAsync(user);

                    if (userDetails != null)
                    {
                        fullName = string.Concat(userDetails.FirstName, " ", userDetails.LastName);
                    }
                })
                .GetAwaiter()
                .GetResult();

            return fullName;
        }
    }
}