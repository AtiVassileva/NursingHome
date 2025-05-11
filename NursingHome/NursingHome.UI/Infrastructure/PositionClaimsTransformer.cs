using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using NursingHome.DAL.Models;
using System.Security.Claims;
using NursingHome.BLL;

namespace NursingHome.UI.Infrastructure
{

    public class PositionClaimsTransformer : IClaimsTransformation
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserService _userService;

        public PositionClaimsTransformer(UserManager<ApplicationUser> userManager, UserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity) principal.Identity!;

            if (!identity.HasClaim(c => c.Type == "position"))
            {
                var user = await _userManager.GetUserAsync(principal);

                if (user is not null)
                {
                    var employee = await _userService.GetById(user.Id);

                    if (employee.EmployeeInfo is not null)
                    {
                        var position = user?.EmployeeInfo?.EmployeePosition.ToString();

                        if (!string.IsNullOrEmpty(position))
                            identity.AddClaim(new Claim("position", position!));
                    }
                }
            }

            return principal;
        }
    }
}