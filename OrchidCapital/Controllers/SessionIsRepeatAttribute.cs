using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrchidCapital.Controllers
{
    public class SessionIsRepeatAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.IsPersistent)?.Value ?? string.Empty;
            if (session != string.Empty && session == "false")
            {
                context.Result = new RedirectToActionResult("ChangePassword", "Login", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}