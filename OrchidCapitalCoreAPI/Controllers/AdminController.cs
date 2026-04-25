using Microsoft.AspNetCore.Mvc;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;
namespace OrchidCapitalCoreAPI.Controllers
{
    [Route("api/ORCHIDCAPITAL")]
    [ApiController]
    [ApiVersion(APIVersions.Version1)]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;
        public AdminController(IAdminRepository adminRepository, IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _configuration = configuration;
        }
        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/GetMasterList")]
        public async Task<IActionResult> GetMasterList([FromBody] GetMasterListRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.GetMasterList(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/EditMasterDetails")]
        public async Task<IActionResult> EditMasterDetails(string UserName, string UserRole, int Id,string SPName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.EditMasterDetails(Id,SPName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/DeleteMasterDetails")]
        public async Task<IActionResult> DeleteMasterDetails([FromBody] DeleteMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.DeleteMasterDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/ChangeStatusInMasterDetails")]
        public async Task<IActionResult> ChangeStatusInMasterDetails([FromBody] ChangeStatusInMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.ChangeStatusInMasterDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdatePageDetails")]
        public async Task<IActionResult> UpdatePageDetails([FromBody] UpdatePageDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdatePageDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateConfigSettingsDetails")]
        public async Task<IActionResult> UpdateConfigSettingsDetails([FromBody] UpdateConfigSettingsDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateConfigSettingsDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateImageConfigDetails")]
        public async Task<IActionResult> UpdateImageConfigDetails([FromBody] UpdateImageConfigDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateImageConfigDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateTeamDetails")]
        public async Task<IActionResult> UpdateTeamDetails([FromBody] UpdateTeamDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateTeamDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateRoleDetails")]
        public async Task<IActionResult> UpdateRoleDetails([FromBody] UpdateRoleDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateRoleDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateUserPageRightsMapping")]
        public async Task<IActionResult> UpdateUserPageRightsMapping([FromBody] UpdateUserPageRightsMappingRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateUserPageRightsMapping(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }
    }
}