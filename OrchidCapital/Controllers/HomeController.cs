using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchidCapital.Helper;
using OrchidCapital.Models;
using OrchidCapital.Resources;

namespace OrchidCapital.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IStringLocalizer<SharedResource> localizer) : base(httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.WelcomeMessage = _localizer["WelcomeMessage"];
            AllModuleListRequest request = new AllModuleListRequest();
            try
            {
                request.ImagesList = await GetImageConfigurationList();
                request.ProductTypesList = await GetLoanProductTypeList();
                request.ProductsList = await GetLoanProductList();
                request.TeamsList = await GetTeamsList();
            }
            catch (Exception ex)
            {

            }
            return View(request);
        }

        private async Task<List<TeamModel>> GetTeamsList()
        {
            List<TeamModel> resultmenu = new List<TeamModel>();
            try
            {
                string QueryStringtype = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=FullName&SortDirection=ASC&SPName=APortal_OrchidCapitalTeamMst&Filter=";
                var responsetype = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryStringtype);
                if (responsetype != null && responsetype.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(responsetype.Response);
                    resultmenu = JsonConvert.DeserializeObject<List<TeamModel>>(jsonResponse);
                }
            }
            catch (Exception ex)
            {

            }
            return resultmenu;
        }

        private async Task<List<BankServiceListModel>> GetLoanProductList()
        {
            List<BankServiceListModel> resultmenu = new List<BankServiceListModel>();
            try
            {
                string QueryStringtype = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=ProductName&SortDirection=ASC&SPName=APortal_LoanProductMaster&Filter=";
                var responsetype = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryStringtype);
                if (responsetype != null && responsetype.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(responsetype.Response);
                    resultmenu = JsonConvert.DeserializeObject<List<BankServiceListModel>>(jsonResponse);
                }
            }
            catch (Exception ex)
            {

            }
            return resultmenu;
        }

        private async Task<List<LoanTypeModel>> GetLoanProductTypeList()
        {
            List<LoanTypeModel> resultmenu = new List<LoanTypeModel>();
            try
            {
                string QueryStringtype = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=Name&SortDirection=ASC&SPName=APortal_LoanProductTypeMst&Filter=";
                var responsetype = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryStringtype);
                if (responsetype != null && responsetype.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(responsetype.Response);
                    resultmenu = JsonConvert.DeserializeObject<List<LoanTypeModel>>(jsonResponse);
                }
            }
            catch (Exception ex)
            {

            }
            return resultmenu;
        }

        private async Task<List<ImageConfigurationModel>> GetImageConfigurationList()
        {
            List<ImageConfigurationModel> resultmenu = new List<ImageConfigurationModel>();
            try
            {
                string QueryStringtype = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=Headline&SortDirection=ASC&SPName=APortal_ImageConfigMst&Filter=";
                var responsetype = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryStringtype);
                if (responsetype != null && responsetype.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(responsetype.Response);
                    resultmenu = JsonConvert.DeserializeObject<List<ImageConfigurationModel>>(jsonResponse);
                }
            }
            catch (Exception ex)
            {

            }
            return resultmenu;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
