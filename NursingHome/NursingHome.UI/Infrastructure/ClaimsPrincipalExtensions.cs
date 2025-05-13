using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NursingHome.DAL.Models;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Infrastructure
{
    using static Common.WebConstants;
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);

        public static bool IsPsychologist(this ClaimsPrincipal user)
            => user.FindFirst("position")?.Value == EmployeePosition.Psychologist.ToString();

        public static bool IsCashier(this ClaimsPrincipal user)
            => user.FindFirst("position")?.Value == EmployeePosition.Cashier.ToString();

        public static bool IsOccupationalTherapist(this ClaimsPrincipal user)
            => user.FindFirst("position")?.Value == EmployeePosition.OccupationalTherapist.ToString();

        public static bool IsSocialWorker(this ClaimsPrincipal user)
            => user.FindFirst("position")?.Value == EmployeePosition.SocialWorker.ToString();

        public static bool IsCook(this ClaimsPrincipal user)
            => user.FindFirst("position")?.Value == EmployeePosition.Cook.ToString();

        public static bool IsNurse(this ClaimsPrincipal user)
            => user.FindFirst("position")?.Value == EmployeePosition.Nurse.ToString();

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