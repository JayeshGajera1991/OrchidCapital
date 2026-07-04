using Dapper;
using Orchid.DataAccess;
using Orchid.DataModels;
using Orchid.TokenValidator;
using OrchidCapitalCoreAPI.Interface;
using System.Data;

namespace OrchidCapitalCoreAPI.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly IConfiguration _configuration;
        public LoginRepository(IConfiguration configuration, IDapperRepository dapperRepository)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _dapperRepository.ConnectionString = SwitchConnectionString.PchaseConnectionstring;
            _dapperRepository.DB_NAME = configuration["DB_NAME"]!;
        }
        public async Task<CommonResponse> AuthenticateUser(LoginRequest request)
        {
            CommonResponse AuthResponse = await UserAuthenticate(request.UserName, request.Password, request.PasswordTryCount);
            return AuthResponse;
        }

        private async Task<CommonResponse> UserAuthenticate(string UserName, string Password, int PasswordTryCount)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.Admin_ValidateUserLogin;
                var param = new DynamicParameters();
                param.Add("@Username", UserName);
                param.Add("@Password", Password);
                param.Add("@PasswordTryCount", PasswordTryCount);

                var queryResponse = await _dapperRepository.GetAllAsync<UserLoginResponse>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    string NewToken = JWTExtension.CreateToken(queryResponse.ToList().FirstOrDefault().UserName, queryResponse.ToList().FirstOrDefault().UserRole, _configuration);
                    queryResponse.ToList().FirstOrDefault().Token = NewToken;
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = Convert.ToString(queryResponse.ToList().FirstOrDefault().Message);
                }
                else
                {
                    response.StatusCode = 0;
                    response.Response = null;
                    response.Message = "Data Not Found.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = null;
            }
            return response;
        }
        private string GetDomainName(string usernameDomain)
        {
            if (string.IsNullOrEmpty(usernameDomain))
            {
                throw (new ArgumentException("Argument can't be null.", "usernameDomain"));
            }
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                return usernameDomain.Substring(0, index);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                return usernameDomain.Substring(index + 1);
            }
            else
            {
                return "";
            }
        }

        private string GetUsername(string usernameDomain)
        {
            if (string.IsNullOrEmpty(usernameDomain))
            {
                throw (new ArgumentException("Argument can't be null.", "usernameDomain"));
            }
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                return usernameDomain.Substring(index + 1);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                return usernameDomain.Substring(0, index);
            }
            else
            {
                return usernameDomain;
            }
        }

        public async Task<CommonResponse> ChangePassword(ChangePassword request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "UPortal_ChangePassword";
                var param = new DynamicParameters();
                param.Add("@CtxName", request.UserName);
                param.Add("@CurPassword", request.CurrentPassword);
                param.Add("@NewPassword", request.NewPassword);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Password changed successfully.";
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

        public async Task<CommonResponse> ForgotPassword(ForgotPasswordViewModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "UPortal_ForgotPasswordRequest";
                var param = new DynamicParameters();
                param.Add("@MailId", request.EmailId);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Password reset link has been sent to your email.";
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

        public async Task<CommonResponse> VerifyResetPasswordRequest(string resetCode)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "UPortal_VerifyResetPasswordRequest";
                var param = new DynamicParameters();
                param.Add("@ResetCode", resetCode);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Reset password request verified successfully.";
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

        public async Task<CommonResponse> ResetPassword(ResetPasswordViewModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "UPortal_ResetPassword";
                var param = new DynamicParameters();
                param.Add("@ResetCode", request.ResetCode);
                param.Add("@EncPassword", request.NewPassword);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "Password reset successfully.";
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
        public async Task<CommonResponse> GetUserPassword(string userName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "UPortal_GetUserPassword";
                var param = new DynamicParameters();
                param.Add("@UserName", userName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "User password retrieved successfully.";
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
