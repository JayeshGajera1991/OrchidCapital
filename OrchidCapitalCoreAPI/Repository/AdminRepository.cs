using System.Data;
using Dapper;
using Orchid.DataAccess;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;
namespace OrchidCapitalCoreAPI.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly IConfiguration _configuration;

        public AdminRepository(IConfiguration configuration, IDapperRepository dapperRepository)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _dapperRepository.ConnectionString = SwitchConnectionString.PchaseConnectionstring;
            _dapperRepository.DB_NAME = configuration["DB_NAME"]!;
        }

        public async Task<CommonResponse> GetMasterList(string UserName, string UserRole, int IsActive, int PageNumber, int PageSize, string SortColumn, string SortDirection, string SPName, string Filter = "")
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "SELECT");
                param.Add("@CreatedBy", UserName);
                param.Add("@Filter", Filter);
                param.Add("@IsActive", IsActive);
                param.Add("@PageNumber", PageNumber);
                param.Add("@PageSize", PageSize);
                param.Add("@SortColumn", SortColumn);
                param.Add("@SortDirection", SortDirection);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> EditMasterDetails(string userName, int id, string SPName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "EDIT");
                param.Add("@Id", id);
                param.Add("@CreatedBy", userName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> DeleteMasterDetails(DeleteMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = request.SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "DELETE");
                param.Add("@Id", request.Id);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> ChangeStatusInMasterDetails(ChangeStatusInMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = request.SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "STATUS");
                param.Add("@Id", request.Id);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdatePageDetails(UpdatePageDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_PageMst;
                var param = new DynamicParameters();
                if (request.Id == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@MenuId", request.MenuId);
                param.Add("@PageName", request.PageName);
                param.Add("@Icon", request.Icon);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@Controller", request.Controller);
                param.Add("@ActionName", request.ActionName);
                param.Add("@OrderNo", request.OrderNo);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateUsersDetails(UpdateUsersDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_UserManage;
                var param = new DynamicParameters();
                if (request.Id == 0)
                {
                    param.Add("@Action", "INSERT");
                }
                else
                {
                    param.Add("@Action", "UPDATE");
                }
                param.Add("@Id", request.Id);
                param.Add("@UserName", request.CreateUserName);
                param.Add("@EmailId", request.EmailId);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@EncPassword", request.EncPassword);
                param.Add("@FirstName", request.FirstName);
                param.Add("@LastName", request.LastName);
                param.Add("@MobileNo", request.MobileNo);
                param.Add("@EmailId", request.EmailId);
                param.Add("@RoleId", request.RoleId);
                param.Add("@Address", request.Address);
                param.Add("@Country", request.Country);
                param.Add("@State", request.State);
                param.Add("@City", request.City);
                param.Add("@ZipCode", request.ZipCode);
                param.Add("@Gender", request.Gender);
                param.Add("@DOB", request.DOB);
                param.Add("@ReferenceCode", request.ReferenceCode);
                param.Add("@EmergencyMobileNumber", request.EmergencyMobileNumber);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateCustomersDetails(UpdateUsersDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_UserManage;
                var param = new DynamicParameters();
                if (request.Id == 0)
                {
                    param.Add("@Action", "INSERT");
                }
                else
                {
                    param.Add("@Action", "UPDATE");
                }
                param.Add("@Id", request.Id);
                param.Add("@UserName", request.CreateUserName);
                param.Add("@EmailId", request.EmailId);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@EncPassword", request.EncPassword);
                param.Add("@FirstName", request.FirstName);
                param.Add("@LastName", request.LastName);
                param.Add("@MobileNo", request.MobileNo);
                param.Add("@EmailId", request.EmailId);
                param.Add("@RoleId", request.RoleId);
                param.Add("@Address", request.Address);
                param.Add("@Country", request.Country);
                param.Add("@State", request.State);
                param.Add("@City", request.City);
                param.Add("@ZipCode", request.ZipCode);
                param.Add("@Gender", request.Gender);
                param.Add("@DOB", request.DOB);
                param.Add("@ReferenceCode", request.ReferenceCode);
                param.Add("@EmergencyMobileNumber", request.EmergencyMobileNumber);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateBranchDetails(UpdateBranchDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_BankBranchMst;
                var param = new DynamicParameters();
                if (request.BranchId == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@BranchId", request.BranchId);
                param.Add("@BranchCode", request.BranchCode);
                param.Add("@BranchName", request.BranchName);
                param.Add("@BranchAddress", request.BranchAddress);
                param.Add("@BankName", request.BankName);
                param.Add("@IFSCCode", request.IFSCCode);
                param.Add("@MICRCode", request.MICRCode);
                param.Add("@ContactPerson", request.ContactPerson);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@Email", request.Email);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateImageConfigDetails(UpdateImageConfigDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_ImageConfigMst;
                var param = new DynamicParameters();
                if (request.Id == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@Headline", request.Headline);
                param.Add("@Description", request.Description);
                param.Add("@Role", request.Role);
                param.Add("@Type", request.Type);
                param.Add("@ImagePath", request.ImagePath);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateTeamDetails(UpdateTeamDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_OrchidCapitalTeamMst;
                var param = new DynamicParameters();
                if (request.Id == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@FullName", request.FullName);
                param.Add("@Headline", request.Headline);
                param.Add("@Description1", request.Description1);
                param.Add("@Role", request.Role);
                param.Add("@Qualification", request.Qualification);
                param.Add("@Description2", request.Description2);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateRoleDetails(UpdateRoleDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_RoleMst;
                var param = new DynamicParameters();
                if (request.Id == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@Role", request.Role);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateUserPageRightsMapping(UpdateUserPageRightsMappingRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_UserPageRightsMappingMst;
                var param = new DynamicParameters();
                param.Add("@UserName", request.UserName);
                param.Add("@XmlData", request.XmlData);
                param.Add("@Id", request.UserId);
                param.Add("@Action", "INSERT");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> ErrorLog(ErrorLogModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_ErrorLog";
                var param = new DynamicParameters();
                param.Add("@UserName", request.UserName);
                param.Add("@ActionName", request.ActionName);
                param.Add("@ControllerName", request.ControllerName);
                param.Add("@ErrorMessage", request.ExceptionMessage);
                param.Add("@Action", "INSERT");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> GetUserPageRightsList(string userName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserPageRightsMappingMst";
                var param = new DynamicParameters();
                param.Add("@UserName", userName);
                param.Add("@Action", "SELECT");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> GetUserLoginList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserPageRightsMappingMst";
                var param = new DynamicParameters();
                param.Add("@Action", "LIST");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> GetRolePermissionsList(int Id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserPageRightsMappingMst";
                var param = new DynamicParameters();
                param.Add("@Action", "EDIT");
                param.Add("@Id", Id);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> TwoFactorAuthentication(TwoFactorRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_TwoFactorAuthentication";
                var param = new DynamicParameters();
                param.Add("@UserName", request.UserName);
                param.Add("@SecretKey", request.SecretKey);
                param.Add("@IsTwoFactor", request.IsTwoFactor);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = request.IsTwoFactor ? "Two Factor Authentication enabled successfully." : "Two Factor Authentication disabled successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> SendWelcomeMail(SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserManage";
                var param = new DynamicParameters();
                param.Add("@UserName", request.SelUserName);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@Action", "sendwcmail");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Your welcome email has been sent successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UnlockUser(SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserManage";
                var param = new DynamicParameters();
                param.Add("@UserName", request.SelUserName);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@Action", "unlockuser");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "The user account has been unlocked successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> ResetPasswordUser(SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserManage";
                var param = new DynamicParameters();
                param.Add("@UserName", request.SelUserName);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@Action", "resetpassword");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "The password for the user has been reset successfully";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> ResetTwoFactorAuthentication(SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserManage";
                var param = new DynamicParameters();
                param.Add("@UserName", request.SelUserName);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@Action", "resettwofactorauth");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "The user's two-factor authentication has been successfully reset";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> DeleteUser(SendWelcomeMailRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserManage";
                var param = new DynamicParameters();
                param.Add("@UserName", request.SelUserName);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@Action", "deleteuser");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "The user account has been deleted successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateProfileImage(UpdateProfileImageRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserManage";
                var param = new DynamicParameters();
                param.Add("@UserName", request.UserName);
                param.Add("@ImageUrl", request.ImageUrl);
                param.Add("@Action", "updateprofileimage");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Your profile image has been updated successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> DeleteProfileImage(DeleteProfileImageRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_UserManage";
                var param = new DynamicParameters();
                param.Add("@UserName", request.UserName);
                param.Add("@Action", "deleteprofileimage");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Your profile image has been deleted successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> GetMenuList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_PageMst";
                var param = new DynamicParameters();
                param.Add("@Action", "MENULIST");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateBankServiceDetails(UpdateBankServiceDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_LoanProductMaster";
                var param = new DynamicParameters();
                if (request.LoanProductId == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@LoanProductId", request.LoanProductId);
                param.Add("@ProductCode", request.ProductCode);
                param.Add("@ProductName", request.ProductName);
                param.Add("@LoanTypeId", request.LoanTypeId);
                param.Add("@MinAmount", request.MinAmount);
                param.Add("@MaxAmount", request.MaxAmount);
                param.Add("@MinTenure", request.MinTenure);
                param.Add("@MaxTenure", request.MaxTenure);
                param.Add("@IsActive", request.IsActive);
                param.Add("@InterestType", request.InterestType);
                param.Add("@DefaultInterestRate", request.DefaultInterestRate);
                param.Add("@ProcessingFeePercent", request.ProcessingFeePercent);
                param.Add("@PenaltyInterest", request.PenaltyInterest);
                param.Add("@GraceDays", request.GraceDays);
                param.Add("@CreatedBy", request.UserName);
                param.Add("@ImageUrl", request.ImageUrl);
                param.Add("@IsSecure", request.IsSecure);
                param.Add("@Descriptions", request.Descriptions);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Your bank service details have been updated successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateLoanTypeDetails(UpdateLoanTypeDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_LoanProductTypeMst";
                var param = new DynamicParameters();
                if (request.LoanTypeId == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@Name", request.Name);
                param.Add("@LoanTypeId", request.LoanTypeId);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Your Loan type details have been updated successfully.";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

    }
}