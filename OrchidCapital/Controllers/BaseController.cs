using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
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
              string   BaseUri = Utility.GetAppSettings("OrchidCoreAPIurl");
            _OrchidClient = new APIClient(BaseUri, Token, UserName, UserRole);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Token = Request.Headers["App-Token"].ToString() == string.Empty ? "" : Request.Headers["App-Token"];
            UserName = Request.Headers["App-UserName"] == string.Empty ? "" : Request.Headers["App-UserName"];
            UserRole = Request.Headers["App-UserRole"] == string.Empty ? "" :Request.Headers["App-UserRole"];
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
