using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchidCapital.Helper;
namespace OrchidCapital.Controllers
{
    public class BaseController : Controller
    {
        protected APIClient _OrchidClient { get; set; }
        protected string UserName { get; set; }
        protected string UserRole { get; set; }
        protected string Token { get; set; }

        protected BaseController(IHttpContextAccessor httpContext, IConfiguration configuration)
        {
            Token = httpContext.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            UserName = httpContext.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? string.Empty;
            UserRole = httpContext.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;
            string BaseUri = Utility.GetAppSettings("OrchidCoreAPIurl");
            _OrchidClient = new APIClient(BaseUri, Token, UserName, UserRole);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Controller controller = context.Controller as Controller;
            if (controller != null)
            {
                //if (string.IsNullOrEmpty(HttpContext.Request.Cookies[ProjectConstants.UserInfoDetails]))
                //{
                //    Response.Cookies.Delete(ProjectConstants.UserInfo);
                //    HttpContext.Session.Clear();
                //    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //    context.Result = new RedirectResult(Utility.GetAppSettings("BasePath") + "/Login/SignOut");
                //    context.HttpContext.Response.StatusCode = 401;
                //}
            }
            base.OnActionExecuting(context);
        }
    }
}
