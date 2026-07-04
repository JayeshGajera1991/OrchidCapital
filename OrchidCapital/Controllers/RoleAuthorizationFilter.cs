using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrchidCapital.Controllers
{
    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;;

            if (!user.Contains("Admin") && !user.Contains("SuperAdmin") && !user.Contains("Power"))
            {
                 context.Result = new ForbidResult();
                return;
            }
        }
    }
}