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
            CommonResponse AuthResponse = new CommonResponse();
            CommonResponse response = new CommonResponse();
            AuthResponse = await UserAuthenticate(request.UserName, request.Password, request.UserRole, request.PasswordTryCount);
            response.StatusCode = AuthResponse.StatusCode;
            response.Response = AuthResponse.Response;
            response.Message = AuthResponse.Message;
            return response;
        }

        private async Task<CommonResponse> UserAuthenticate(string UserName, string Password, string UserRole, int PasswordTryCount, bool IsEncryptedPassword = false)
        {
            CommonResponse objResponse = new CommonResponse();
            List<AuthUserLogin> objlist = new List<AuthUserLogin>();
            AuthUserLogin authUser = new AuthUserLogin();
            string userId = string.Empty;
            string FullName = string.Empty;
            string Email = string.Empty;
            int LoginTryCount = 0;
            try
            {
                if (UserRole.ToLower() == "admin")
                {
                    if (UserName.ToLower() == "orchidcapitaladmin" && Password == "Admin$$2822" && UserRole.ToLower() == "admin")
                    {
                        userId = "1";
                        authUser.UserName = UserName;
                        authUser.FullName = UserName;
                        authUser.UserRole = UserRole;
                    }
                    else
                    {
                        userId = "-1";
                    }
                    //var query = ClientPortalStoreProcedureMappings.NewPortal_Validate_UserLogin_API;
                    //var param = new DynamicParameters();
                    //param.Add("@Username", UserName);
                    //param.Add("@PasswordTryCount", PasswordTryCount);
                    //param.Add("@UserType", UserType);

                    //var queryResponse = await _dapperRepository.GetAllAsync<UserLoginResponse>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                    //CommonResponse response = new CommonResponse();
                    //if (queryResponse.Any())
                    //{
                    //    response.StatusCode = 1;
                    //    response.Response = queryResponse;
                    //    response.Message = Convert.ToString(response.Response[0].Message);
                    //    userId = Convert.ToString(response.Response[0].Message);
                    //    LoginTryCount = Convert.ToInt32(response.Response[0].LoginTryCount);
                    //    authUser.Email = response.Response[0].Email;
                    //    authUser.UserName = Convert.ToString(response.Response[0].UserName);
                    //    authUser.FullName = Convert.ToString(response.Response[0].FullName);
                    //    authUser.IsRepeat = Convert.ToInt32(response.Response[0].IsRepeat);
                    //}
                    //else
                    //{
                    //    response.StatusCode = 0;
                    //    response.Message = "Data Not Found.";
                    //}
                }
                switch (userId)
                {
                    case "-1":
                        objResponse.StatusCode = 0;
                        if (UserRole.ToLower() == "admin")
                        {
                            objResponse.Message = "101";
                        }
                        else
                        {
                            objResponse.Message = "102";
                        }
                        objResponse.Response = Convert.ToString(LoginTryCount);
                        break;
                    case "-2":
                        objResponse.StatusCode = 2;
                        objResponse.Message = "103";
                        objResponse.Response = null;
                        break;
                    case "-5":
                        objResponse.StatusCode = 5;
                        objResponse.Response = null;
                        objResponse.Message = "104";
                        break;
                    case "-3":
                        objResponse.StatusCode = 3;
                        objResponse.Response = null;
                        objResponse.Message = "105";
                        break;
                    case "-4":
                        objResponse.StatusCode = 4;
                        objResponse.Response = null;
                        objResponse.Message = "106";
                        break;
                    default:
                        string response = JWTExtension.CreateToken(authUser.UserName, UserRole, _configuration);
                        objlist.Add(new AuthUserLogin
                        {
                            Token = response,
                            UserName = authUser.UserName,
                            FullName = authUser.FullName,
                            EmailId = authUser.EmailId,
                            UserRole = authUser.UserRole
                        });

                        objResponse.StatusCode = 1;
                        objResponse.Response = objlist;
                        objResponse.Message = "";
                        break;
                }
            }

            catch (Exception ex)
            {
                objResponse.StatusCode = 0;
                objResponse.Message = ex.Message;
                objResponse.Response = "Internal Server Error";
            }
            return objResponse;
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
    }
}
