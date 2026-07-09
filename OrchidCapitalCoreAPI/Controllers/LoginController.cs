using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orchid.DataModels;
using Orchid.EmailService.Interface;
using Orchid.UtilityHelper;
using OrchidCapitalCoreAPI.Interface;

namespace OrchidCapitalCoreAPI.Controllers
{
    [Route("api/ORCHIDCAPITAL")]
    [ApiController]
    [ApiVersion(APIVersions.Version1)]
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly OrchidCapitalCoreAPI.Helper.RsaService _rsaService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public LoginController(ILoginRepository loginRepository, IConfiguration configuration, IEmailService emailService, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _loginRepository = loginRepository;
            _configuration = configuration;
            _emailService = emailService;
            _environment = environment;
        }

        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpPost("v{version:apiVersion}/AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginRequest request)
        {
            // Attempt to decrypt password if client sent RSA-encrypted value
            try
            {
                if (!string.IsNullOrEmpty(request?.Password))
                {
                    var rsa = HttpContext.RequestServices.GetService(typeof(OrchidCapitalCoreAPI.Helper.RsaService)) as OrchidCapitalCoreAPI.Helper.RsaService;
                    if (rsa != null)
                    {
                        var maybe = rsa.TryDecrypt(request.Password);
                        if (!string.IsNullOrEmpty(maybe))
                        {
                            request.Password = maybe;
                        }
                    }
                }
            }
            catch { }
            CommonResponse response = new CommonResponse();
            Tuple<bool, string> tpl = null;
            try
            {
                response = await _loginRepository.AuthenticateUser(request);
                if (response.Message == "-1")
                {
                    string jsonResponse = JsonSerializer.Serialize(response.Response);
                    List<AuthUserLogin> authUserLogins = JsonSerializer.Deserialize<List<AuthUserLogin>>(jsonResponse);
                    var webRootPath = _environment.WebRootPath;
                    var pathToFile = webRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplates" + Path.DirectorySeparatorChar.ToString() + "AccountLocked.html";
                    var emailTemplate = System.IO.File.ReadAllText(pathToFile);
                    emailTemplate = emailTemplate.Replace("{{UserName}}", authUserLogins?.FirstOrDefault()?.FullName);
                    emailTemplate = emailTemplate.Replace("{{weblink}}", Convert.ToString(_configuration.GetSection("EmailConfiguration:OrgURL").Value));
                    EmailRequestModel model = new EmailRequestModel
                    {
                        EmailBCC = "",
                        EmailCC = "",
                        EmailBody = emailTemplate,
                        EmailFrom = Convert.ToString(_configuration.GetSection("EmailConfiguration:From").Value),
                        EmailSubject = Convert.ToString(_configuration.GetSection("EmailConfiguration:Userlock").Value),
                        EmailToName = Convert.ToString(_configuration.GetSection("EmailConfiguration:Userlock").Value),
                        EmailToId = Convert.ToString(authUserLogins?.FirstOrDefault()?.EmailId)
                    };
                    tpl = await _emailService.SendEmailAsync(model);

                    if (tpl != null && tpl.Item1 == false)
                    {
                        response.Message = "User authenticated successfully, but email sending failed with error: " + tpl.Item2;
                        response.StatusCode = 500; // Custom status code to indicate email sending failure
                        response.Response = null; // Clear the response data if email sending failed
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword request)
        {
            CommonResponse response = new CommonResponse();
            Tuple<bool, string> tpl = null;
            try
            {
                response = await _loginRepository.ChangePassword(request);
                if (response.StatusCode == 1)
                {
                    string jsonResponse = JsonSerializer.Serialize(response.Response);
                    List<AuthUserLogin> authUserLogins = JsonSerializer.Deserialize<List<AuthUserLogin>>(jsonResponse);
                    var webRootPath = _environment.WebRootPath;
                    var pathToFile = webRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplates" + Path.DirectorySeparatorChar.ToString() + "ChangePassword.html";
                    var emailTemplate = System.IO.File.ReadAllText(pathToFile);
                    emailTemplate = emailTemplate.Replace("$$FullName$$", authUserLogins?.FirstOrDefault()?.FullName);
                    emailTemplate = emailTemplate.Replace("$$LoginLink$$", Convert.ToString(_configuration.GetSection("EmailConfiguration:OrgURL").Value) + "Login/Login");
                    EmailRequestModel model = new EmailRequestModel
                    {
                        EmailBCC = "",
                        EmailCC = "",
                        EmailBody = emailTemplate,
                        EmailFrom = Convert.ToString(_configuration.GetSection("EmailConfiguration:From").Value),
                        EmailSubject = Convert.ToString(_configuration.GetSection("EmailConfiguration:ChangePassword").Value),
                        EmailToName = Convert.ToString(_configuration.GetSection("EmailConfiguration:ChangePassword").Value),
                        EmailToId = Convert.ToString(authUserLogins?.FirstOrDefault()?.EmailId)
                    };
                    tpl = await _emailService.SendEmailAsync(model);

                    if (tpl != null && tpl.Item1 == false)
                    {
                        response.Message = "User authenticated successfully, but email sending failed with error: " + tpl.Item2;
                        response.StatusCode = 500; // Custom status code to indicate email sending failure
                        response.Response = null; // Clear the response data if email sending failed
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpPost("v{version:apiVersion}/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel request)
        {
            CommonResponse response = new CommonResponse();
            Tuple<bool, string> tpl = null;
            try
            {
                response = await _loginRepository.ForgotPassword(request);
                if (response.StatusCode == 1)
                {
                    string jsonResponse = JsonSerializer.Serialize(response.Response);
                    List<AuthUserLogin> authUserLogins = JsonSerializer.Deserialize<List<AuthUserLogin>>(jsonResponse);
                    var webRootPath = _environment.WebRootPath;
                    var pathToFile = webRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplates" + Path.DirectorySeparatorChar.ToString() + "ForgotPassword.html";
                    var emailTemplate = System.IO.File.ReadAllText(pathToFile);
                    emailTemplate = emailTemplate.Replace("$$FullName$$", authUserLogins?.FirstOrDefault()?.FullName);
                    emailTemplate = emailTemplate.Replace("$$ResetLink$$", Convert.ToString(_configuration.GetSection("EmailConfiguration:OrgURL").Value) + "Admin/ResetPassword?token=" + authUserLogins?.FirstOrDefault()?.ResetCode);
                    EmailRequestModel model = new EmailRequestModel
                    {
                        EmailBCC = "",
                        EmailCC = "",
                        EmailBody = emailTemplate,
                        EmailFrom = Convert.ToString(_configuration.GetSection("EmailConfiguration:From").Value),
                        EmailSubject = Convert.ToString(_configuration.GetSection("EmailConfiguration:ForgotPassword").Value),
                        EmailToName = Convert.ToString(_configuration.GetSection("EmailConfiguration:ForgotPassword").Value),
                        EmailToId = Convert.ToString(authUserLogins?.FirstOrDefault()?.EmailId)
                    };
                    tpl = await _emailService.SendEmailAsync(model);

                    if (tpl != null && tpl.Item1 == false)
                    {
                        response.Message = "User authenticated successfully, but email sending failed with error: " + tpl.Item2;
                        response.StatusCode = 500; // Custom status code to indicate email sending failure
                        response.Response = null; // Clear the response data if email sending failed
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpGet("v{version:apiVersion}/VerifyResetPasswordRequest")]
        public async Task<IActionResult> VerifyResetPasswordRequest(string ResetCode)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _loginRepository.VerifyResetPasswordRequest(ResetCode);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpPost("v{version:apiVersion}/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel request)
        {
            CommonResponse response = new CommonResponse();
            Tuple<bool, string> tpl = null;
            try
            {
                response = await _loginRepository.ResetPassword(request);
                if (response.StatusCode == 1)
                {
                    string jsonResponse = JsonSerializer.Serialize(response.Response);
                    List<AuthUserLogin> authUserLogins = JsonSerializer.Deserialize<List<AuthUserLogin>>(jsonResponse);
                    var webRootPath = _environment.WebRootPath;
                    var pathToFile = webRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplates" + Path.DirectorySeparatorChar.ToString() + "ResetPassword.html";
                    var emailTemplate = System.IO.File.ReadAllText(pathToFile);
                    emailTemplate = emailTemplate.Replace("$$FullName$$", authUserLogins?.FirstOrDefault()?.FullName);
                    emailTemplate = emailTemplate.Replace("$$LoginLink$$", Convert.ToString(_configuration.GetSection("EmailConfiguration:OrgURL").Value) + "Login/Login");
                    EmailRequestModel model = new EmailRequestModel
                    {
                        EmailBCC = "",
                        EmailCC = "",
                        EmailBody = emailTemplate,
                        EmailFrom = Convert.ToString(_configuration.GetSection("EmailConfiguration:From").Value),
                        EmailSubject = Convert.ToString(_configuration.GetSection("EmailConfiguration:ForgotPassword").Value),
                        EmailToName = Convert.ToString(_configuration.GetSection("EmailConfiguration:ForgotPassword").Value),
                        EmailToId = Convert.ToString(authUserLogins?.FirstOrDefault()?.EmailId)
                    };
                    tpl = await _emailService.SendEmailAsync(model);

                    if (tpl != null && tpl.Item1 == false)
                    {
                        response.Message = "User authenticated successfully, but email sending failed with error: " + tpl.Item2;
                        response.StatusCode = 500; // Custom status code to indicate email sending failure
                        response.Response = null; // Clear the response data if email sending failed
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }
        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpGet("v{version:apiVersion}/GetUserPassword")]
        public async Task<IActionResult> GetUserPassword(string UserName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _loginRepository.GetUserPassword(UserName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }
    
        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpGet("v{version:apiVersion}/EncryptText")]
        public IActionResult EncryptText(string text)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response.Message = Encryption.Encrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), text);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpGet("v{version:apiVersion}/DecryptText")]
        public IActionResult DecryptText(string text)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response.Message = Encryption.Decrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), text);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

    }
}
