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
        [HttpGet("v{version:apiVersion}/GetMasterList")]
        public async Task<IActionResult> GetMasterList(string UserName, string UserRole, int IsActive, int PageNumber, int PageSize, string SortColumn, string SortDirection, string SPName, string Filter = "")
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.GetMasterList(UserName, UserRole, IsActive, PageNumber, PageSize, SortColumn, SortDirection, SPName, Filter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/EditMasterDetails")]
        public async Task<IActionResult> EditMasterDetails(string UserName, string UserRole, int Id, string SPName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.EditMasterDetails(UserName, Id, SPName);
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
        [HttpPost("v{version:apiVersion}/UpdateBranchDetails")]
        public async Task<IActionResult> UpdateBranchDetails([FromBody] UpdateBranchDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateBranchDetails(request);
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
        [HttpPost("v{version:apiVersion}/UpdateUsersDetails")]
        public async Task<IActionResult> UpdateUsersDetails([FromBody] UpdateUsersDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateUsersDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateCustomersDetails")]
        public async Task<IActionResult> UpdateCustomersDetails([FromBody] UpdateUsersDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateCustomersDetails(request);
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

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/ErrorLog")]
        public async Task<IActionResult> ErrorLog([FromBody] ErrorLogModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.ErrorLog(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetUserPageRightsList")]
        public async Task<IActionResult> GetUserPageRightsList(string UserName, string UserRole)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.GetUserPageRightsList(UserName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetUserLoginList")]
        public async Task<IActionResult> GetUserLoginList(string UserName, string UserRole)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.GetUserLoginList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetRolePermissionsList")]
        public async Task<IActionResult> GetRolePermissionsList(string UserName, string UserRole, int Id = 0)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.GetRolePermissionsList(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/TwoFactorAuthentication")]
        public async Task<IActionResult> TwoFactorAuthentication([FromBody] TwoFactorRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.TwoFactorAuthentication(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/SendWelcomeMail")]
        public async Task<IActionResult> SendWelcomeMail([FromBody] SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.SendWelcomeMail(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UnlockUser")]
        public async Task<IActionResult> UnlockUser([FromBody] SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UnlockUser(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/ResetPasswordUser")]
        public async Task<IActionResult> ResetPasswordUser([FromBody] SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.ResetPasswordUser(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/ResetTwoFactorAuthentication")]
        public async Task<IActionResult> ResetTwoFactorAuthentication([FromBody] SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.ResetTwoFactorAuthentication(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.DeleteUser(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage([FromBody] UpdateProfileImageRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateProfileImage(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/DeleteProfileImage")]
        public async Task<IActionResult> DeleteProfileImage([FromBody] DeleteProfileImageRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.DeleteProfileImage(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetMenuList")]
        public async Task<IActionResult> GetMenuList(string UserName, string UserRole)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.GetMenuList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateBankServiceDetails")]
        public async Task<IActionResult> UpdateBankServiceDetails([FromBody] UpdateBankServiceDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateBankServiceDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateLoanTypeDetails")]
        public async Task<IActionResult> UpdateLoanTypeDetails([FromBody] UpdateLoanTypeDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _adminRepository.UpdateLoanTypeDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

    }
}