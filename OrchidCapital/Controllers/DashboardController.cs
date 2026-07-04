using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrchidCapital.Helper;
using OrchidCapital.Models;

namespace OrchidCapital.Controllers
{
    [SessionIsRepeat]
    [SessionTimeOut]
    public class DashboardController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public DashboardController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<IActionResult> Dashboard()
        {
            DashboardCommonModel result = new DashboardCommonModel();
            try
            {
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetToolsCountsList + QueryString);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    result.ToolsCountsList = JsonConvert.DeserializeObject<List<GetToolsCountsListModel>>(jsonResponse);
                }

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "DashboardController", "Dashboard");
            }
            return View(result);
        }
        
        #region Error
        private async Task SaveErrorLog(Exception ex, string v1, string v2)
        {
            try
            {
                ErrorLogModel errorLog = new ErrorLogModel
                {
                    UserName = UserName,
                    UserRole = UserRole,
                    ControllerName = v1,
                    ActionName = v2,
                    ExceptionMessage = ex.Message
                };
                await _OrchidClient.PostAsync<dynamic>(ProxyAPI.ErrorLog, errorLog);
            }
            catch (Exception ex1)
            {
                await SaveErrorLog(ex1, "AdminController", "SaveErrorLog");
            }
        }
        #endregion

    }
}