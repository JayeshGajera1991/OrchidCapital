using Microsoft.AspNetCore.Mvc;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;

namespace OrchidCapitalCoreAPI.Controllers
{
    [Route("api/ORCHIDCAPITAL")]
    [ApiController]
    [ApiVersion(APIVersions.Version1)]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IConfiguration _configuration;
        public DashboardController(IDashboardRepository dashboardRepository, IConfiguration configuration)
        {
            _dashboardRepository = dashboardRepository;
            _configuration = configuration;
        }
        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetToolsCountsList")]
        public async Task<IActionResult> GetToolsCountsList(string UserName, string UserRole)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _dashboardRepository.GetToolsCountsList(UserName, UserRole);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

    }
}