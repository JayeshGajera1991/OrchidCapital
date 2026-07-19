using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrchidCapital.Models;

namespace OrchidCapital.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> AboutUs()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> ContactUs()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> Leadership()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> Careers()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> Newsroom()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> BusinessLoans()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> StructuredFinance()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> WorkingCapital()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> Advisory()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> Compliance()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> Disclosures()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public IActionResult PrivacyPolicy()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public IActionResult UserRequest()
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            var supportedCultures = new[] { "en", "hi", "gu", "mr" };

            if (!supportedCultures.Contains(culture))
            {
                culture = "en";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)
                ),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                }
            );

            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }
    }
}