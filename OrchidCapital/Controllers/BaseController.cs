using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OrchidCapital.Helper;
using OrchidCapital.Models;
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
        public override async Task OnActionExecutionAsync(
       ActionExecutingContext context,
       ActionExecutionDelegate next)
        {
            AllModuleListRequest request = new AllModuleListRequest
            {
                ImagesList = await GetImageConfigurationList(),
                ProductTypesList = await GetLoanProductTypeList(),
                ProductsList = await GetLoanProductList(),
                TeamsList = await GetTeamsList()
            };

            ViewBag.HeaderData = request;

            await next();
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

    }
}
