using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OrchidCapital.Models;
using OrchidCapital.Helper;
namespace OrchidCapital.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly APIClient _OrchidClient;
        private string APIBaseUrl = Utility.GetAppSettings("OrchidCoreAPIurl");
        public LoginController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _OrchidClient = new APIClient(APIBaseUrl, string.Empty, string.Empty, string.Empty);
        }
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            if (!string.IsNullOrEmpty(Utility.GetCookie("UserName", _httpContextAccessor)) && !string.IsNullOrEmpty(Utility.GetCookie("Password", _httpContextAccessor)))
            {
                model.UserName = Utility.GetCookie("UserName", _httpContextAccessor);
                model.Password = Utility.GetCookie("Password", _httpContextAccessor);
                model.RememberMe = true;
            }
            else
            {
                model.UserName = string.Empty;
                model.Password = string.Empty;
                model.RememberMe = false;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                if(model.RememberMe)
                {
                    Utility.SetCookie("UserName", model.UserName, 5, _httpContextAccessor);
                    Utility.SetCookie("Password", model.Password, 5, _httpContextAccessor);
                }
                else
                {
                    Utility.RemoveCookie("UserName", _httpContextAccessor);
                    Utility.RemoveCookie("Password", _httpContextAccessor);
                }
                model.Password = Encryption.Encrypt(_configuration["USR-ENC-KEY"], model.Password);
                var result = await _OrchidClient.PostAsync<LoginResponse>(ProxyAPI.AuthenticateUser, model);
                var lst = new List<LoginResponse>();
                if(result != null && result.IsSuccessStatusCode)
                {
                   lst= result.Result;
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(ClaimTypes.Role, lst.ToList().FirstOrDefault().UserRole)
                };

                var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var Props = new AuthenticationProperties()
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                };
                Thread.CurrentPrincipal = principal;
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,Props);

                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }
        }

        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}