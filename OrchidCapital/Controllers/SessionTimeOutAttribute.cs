using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrchidCapital.Controllers
{
    public class SessionTimeOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? string.Empty;
            if (session != string.Empty && !string.IsNullOrEmpty(context.HttpContext.Request.Cookies["Cookies"]))
            {
                context.HttpContext.Response.Cookies.Delete("Cookies");
                context.HttpContext.Session.Clear();
                context.HttpContext.SignOutAsync("Cookies").Wait();
                context.Result = new RedirectToActionResult("Login", "Login", null);
                context.HttpContext.Response.StatusCode = 401;
            }
            base.OnActionExecuting(context);
        }
    }
}