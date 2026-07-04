using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrchidCapital.Helper;
using OrchidCapital.Models;
using TwoFactorAuthNet;

namespace OrchidCapital.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly APIClient _OrchidClient;
        private readonly RsaService _rsaService;
        private string APIBaseUrl = Utility.GetAppSettings("OrchidCoreAPIurl");

        public LoginController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            RsaService rsaService
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _rsaService = rsaService;
            _OrchidClient = new APIClient(APIBaseUrl, string.Empty, string.Empty, string.Empty);
        }

        #region Login and Logout
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                string key = Environment.GetEnvironmentVariable("USR_ENC_KEY");
                if (string.IsNullOrEmpty(key))
                {
                    throw new Exception("USR_ENC_KEY not found");
                }
                if (
                    !string.IsNullOrEmpty(Utility.GetCookie("UserName", _httpContextAccessor))
                    && !string.IsNullOrEmpty(Utility.GetCookie("Password", _httpContextAccessor))
                )
                {
                    model.UserName = Utility.GetCookie("UserName", _httpContextAccessor);
                    model.Password = Utility.GetCookie("Password", _httpContextAccessor);
                    model.RememberMe = true;
                }
                else
                {
                    model.UserName = string.Empty;
                    model.Password = string.Empty;
                    model.RememberMe = false;
                }
                model.PublicKey = _rsaService.GetPublicKey();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Login: " + ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
                {
                    if (!string.IsNullOrEmpty(Request.Form["hdnEncryptedPassword"].ToString()))
                    {
                        model.Password = _rsaService.Decrypt(
                            Request.Form["hdnEncryptedPassword"].ToString()
                        );
                    }
                    if (!model.Password.StartsWith("*******"))
                    {
                        // Remember Me (Save Username & Password in Cookie)
                        if (model.RememberMe)
                        {
                            Utility.SetCookie("UserName", model.UserName, 5, _httpContextAccessor);
                            Utility.SetCookie("Password", model.Password, 5, _httpContextAccessor);
                        }
                        else
                        {
                            Utility.RemoveCookie("UserName", _httpContextAccessor);
                            Utility.RemoveCookie("Password", _httpContextAccessor);
                        }
                        // Check Password (Current Passswor & Database Password)
                        string QueryString = $"?UserName={model.UserName}";
                        var response = await _OrchidClient.GetAsync<dynamic>(
                            ProxyAPI.GetUserPassword + QueryString
                        );
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string encryptedPassword = response.Response[0].EncPassword.ToString();
                            string decryptedPassword = Encryption.Decrypt(
                                Environment.GetEnvironmentVariable("USR_ENC_KEY"),
                                encryptedPassword
                            );
                            if (model.Password != decryptedPassword)
                            {
                                model.ErrorMessage = "Invalid username or password.";
                                return View(model);
                            }
                        }
                        else
                        {
                            model.ErrorMessage = "Invalid username or password.";
                            return View(model);
                        }
                        // Encrypt Password before sending to API
                        model.Password = Encryption.Encrypt(
                            Environment.GetEnvironmentVariable("USR_ENC_KEY"),
                            model.Password
                        );
                        // Call API to Authenticate User
                        var result = await _OrchidClient.PostAsync<LoginResponse>(
                            ProxyAPI.AuthenticateUser,
                            model
                        );
                        var lst = new List<LoginResponse>();
                        if (result != null && result.IsSuccessStatusCode)
                        {
                            lst = result.Result;
                            if (lst == null || lst.Count == 0)
                            {
                                model.ErrorMessage = "Invalid username or password.";
                                return View(model);
                            }
                            if (result.Message == "1")
                            {
                                List<MenuItemVm> lstMenuItems = new List<MenuItemVm>();
                                string QueryString1 =
                                    @"?UserName="
                                    + lst.ToList().FirstOrDefault().UserName
                                    + "&UserRole="
                                    + lst.ToList().FirstOrDefault().UserRole;
                                var response1 = await _OrchidClient.GetAsync<dynamic>(
                                    ProxyAPI.GetUserPageRightsList + QueryString1
                                );
                                if (response1 != null && response1.IsSuccessStatusCode)
                                {
                                    string jsonResponse1 = JsonConvert.SerializeObject(
                                        response1.Response
                                    );
                                    List<GetRolePermissionsList> result2 =
                                        JsonConvert.DeserializeObject<List<GetRolePermissionsList>>(
                                            jsonResponse1
                                        );
                                    if (result2 != null && result2.Count > 0)
                                    {
                                        foreach (GetRolePermissionsList x in result2)
                                        {
                                            MenuItemVm menuItem = new MenuItemVm
                                            {
                                                Id = x.Id,
                                                PageName = x.PageName,
                                                Icon = x.Icon,
                                                Controller = x.Controller,
                                                Action = x.ActionName,
                                                Section = x.MenuName,
                                                IsSelected = x.IsSelected,
                                            };
                                            lstMenuItems.Add(menuItem);
                                        }
                                    }
                                }
                                var claims = new List<Claim>
                                {
                                    new Claim(
                                        ClaimTypes.Name,
                                        lst.ToList().FirstOrDefault().UserName
                                    ),
                                    new Claim(
                                        ClaimTypes.Role,
                                        lst.ToList().FirstOrDefault().UserRole
                                    ),
                                     new Claim(
                                        ClaimTypes.Surname,
                                        lst.ToList().FirstOrDefault().ReferenceCode
                                    ),
                                     new Claim(
                                        ClaimTypes.GivenName,
                                        lst.ToList().FirstOrDefault().ImageUrl
                                    ),
                                    new Claim(
                                        ClaimTypes.IsPersistent,
                                        lst.ToList().FirstOrDefault().IsRepeat.ToString().ToLower()
                                    ),
                                    new Claim(
                                        ClaimTypes.NameIdentifier,
                                        lst.ToList().FirstOrDefault().Token
                                    ),
                                    new Claim(
                                        ClaimTypes.UserData,
                                        Encryption.Encrypt(
                                            Environment.GetEnvironmentVariable("USR_ENC_KEY"),
                                            JsonConvert.SerializeObject(lstMenuItems)
                                        )
                                    ),
                                };

                                var identity = new ClaimsIdentity(
                                    claims,
                                    CookieAuthenticationDefaults.AuthenticationScheme
                                );

                                var principal = new ClaimsPrincipal(identity);
                                var Props = new AuthenticationProperties()
                                {
                                    IsPersistent = model.RememberMe,
                                    ExpiresUtc = DateTime.UtcNow.AddDays(1),
                                };
                                Thread.CurrentPrincipal = principal;
                                await _httpContextAccessor.HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    principal,
                                    Props
                                );
                                if (lst.ToList().FirstOrDefault().IsTwoFactor)
                                {
                                    HttpContext.Session.SetString(
                                        "SecretKey",
                                        lst.ToList().FirstOrDefault().SecretKey
                                    );
                                    HttpContext.Session.SetString(
                                        "UserName",
                                        lst.ToList().FirstOrDefault().UserName
                                    );
                                    return RedirectToAction("TwoFactorVerification", "Login");
                                }
                                if (lst.ToList().FirstOrDefault().IsRepeat == false)
                                {
                                    return RedirectToAction("ChangePassword", "Admin");
                                }
                                return RedirectToAction("Dashboard", "Dashboard");
                            }
                            if (result.Message == "-1")
                            {
                                model.ErrorMessage =
                                    "User account is locked. Please contact administrator.";
                                return View(model);
                            }
                            if (result.Message == "-2")
                            {
                                model.ErrorMessage =
                                    "User account is not activated. Please contact administrator.";
                            }
                            else if (result.Message == "-3")
                            {
                                model.ErrorMessage = "Invalid username or password.";
                            }
                            else if (result.Message == "-4")
                            {
                                model.ErrorMessage = "Invalid username or password.";
                            }
                            else if (result.Message == "-5")
                            {
                                model.ErrorMessage =
                                    "User account has expired. Please contact administrator.";
                            }
                            else
                            {
                                model.ErrorMessage = "Invalid username or password.";
                            }
                            return View(model);
                        }
                        else
                        {
                            model.ErrorMessage = "Invalid username or password.";
                            return View(model);
                        }
                    }
                    else
                    {
                        model.ErrorMessage = "Invalid username or password.";
                        return View(model);
                    }
                }
                else
                {
                    model.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage =
                    "An error occurred while processing your request. Please try again later.";
                return View(model);
            }
        }
        #endregion

        #region Single Sign-On (SSO) with Azure AD
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _httpContextAccessor.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            return RedirectToAction("Login");
        }
        #endregion

        #region Two Factor Authentication
        public async Task<IActionResult> TwoFactorVerification()
        {
            TwoFactorViewModel model = new TwoFactorViewModel();
            model.UserName = HttpContext.Session.GetString("UserName");
            model.SecretKey = HttpContext.Session.GetString("SecretKey");
            model.UserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            model.ErrorMessage = "";
            if (string.IsNullOrEmpty(model.SecretKey) || string.IsNullOrEmpty(model.UserName))
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactorVerification(TwoFactorViewModel model)
        {
            try
            {
                if (
                    !string.IsNullOrEmpty(model.SecretKey)
                    && !string.IsNullOrEmpty(model.UserName)
                    && !string.IsNullOrEmpty(model.AuthCode)
                )
                {
                    var tfa = new TwoFactorAuth();
                    bool isValid = tfa.VerifyCode(model.SecretKey, model.AuthCode);
                    if (isValid)
                    {
                        HttpContext.Session.Remove("SecretKey");
                        HttpContext.Session.Remove("UserName");
                    }
                    else
                    {
                        model.ErrorMessage = "Invalid verification code. Please try again.";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage =
                    "An error occurred while processing your request. Please try again later.";
                return View(model);
            }
            return RedirectToAction("Dashboard", "Dashboard");
        }
        #endregion

        #region RSA Key Generation
        public IActionResult EncryptRSAKeys(string text)
        {
            try
            {
                _rsaService.Encrypt(text);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =
                    "An error occurred while generating RSA keys. Please try again later.";
            }
            return View();
        }

        public IActionResult DecryptRSAKeys()
        {
            try { }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =
                    "An error occurred while generating RSA keys. Please try again later.";
            }
            return View();
        }
        #endregion

        #region Change Password
        public async Task<IActionResult> ChangePassword()
        {
            ChangePasswordViewModel request = new ChangePasswordViewModel();
            try
            {
                request.UserName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                request.UserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                request.PublicKey = _rsaService.GetPublicKey();
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "LoginController", "ChangePassword");
            }
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(Request.Form["hdnEncryptedCurrentPassword"].ToString()))
                    {
                        request.CurrentPassword = _rsaService.Decrypt(Request.Form["hdnEncryptedCurrentPassword"].ToString());
                    }
                    if (!string.IsNullOrEmpty(Request.Form["hdnEncryptedNewPassword"].ToString()))
                    {
                        request.NewPassword = _rsaService.Decrypt(Request.Form["hdnEncryptedNewPassword"].ToString());
                    }
                    if (!request.CurrentPassword.StartsWith("*******") && !request.NewPassword.StartsWith("*******"))
                    {
                        // Check Password (Current Passswor & Database Password)
                        string QueryString = $"?UserName={User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value}";
                        var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetUserPassword + QueryString);
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string encryptedPassword = response.Response[0].EncPassword.ToString();
                            string decryptedPassword = Encryption.Decrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), encryptedPassword);
                            if (request.CurrentPassword != decryptedPassword)
                            {
                                request.ErrorMessage = "Invalid username or password.";
                                return View(request);
                            }
                        }
                        else
                        {
                            request.ErrorMessage = "Invalid username or password.";
                            return View(request);
                        }

                        request.UserName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                        request.UserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                        request.CurrentPassword = Encryption.Encrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), request.CurrentPassword);
                        request.NewPassword = Encryption.Encrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), request.NewPassword);
                        var response1 = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.ChangePassword, request);
                        if (response1 != null && response1.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Dashboard", "Dashboard");
                        }
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return RedirectToAction("ChangePassword", "Login");
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "LoginController", "ChangePassword");
            }
            return RedirectToAction("Dashboard", "Dashboard");
        }
        #endregion


        #region Error
        private async Task SaveErrorLog(Exception ex, string v1, string v2)
        {
            try
            {
                ErrorLogModel errorLog = new ErrorLogModel
                {
                    UserName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                    UserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value,
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
