using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrchidCapital.Helper;
using OrchidCapital.Models;
using TwoFactorAuthNet;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
namespace OrchidCapital.Controllers
{
    [SessionIsRepeat]
    [SessionTimeOut]
    public class AdminController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly RsaService _rsaService;
        private readonly IWebHostEnvironment _environment;
        public AdminController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, RsaService rsaService, IWebHostEnvironment environment) : base(httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _rsaService = rsaService;
            _environment = environment;
        }

        #region Profile | Update Profile Image | Delete Profile Image | Edit User Details
        public async Task<IActionResult> Profile()
        {
            GetUserDetailsResponseModel result = new GetUserDetailsResponseModel();
            try
            {
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + 0 + "&SPName=" + "APortal_UserManage";
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    List<GetUserDetailsModel> result1 = JsonConvert.DeserializeObject<List<GetUserDetailsModel>>(jsonResponse);
                    result.SingleResult = result1.FirstOrDefault();
                }

                string QueryString1 = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                var response1 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetUserPageRightsList + QueryString1);
                if (response1 != null && response1.IsSuccessStatusCode)
                {
                    string jsonResponse1 = JsonConvert.SerializeObject(response1.Response);
                    List<GetUserPageRightsList> result2 = JsonConvert.DeserializeObject<List<GetUserPageRightsList>>(jsonResponse1);
                    result.PageRights = result2;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Profile");
            }
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (request.ImageFile != null && request.ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "ImageProfiler");
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".jfif" };
                        var extension = Path.GetExtension(request.ImageFile.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            return Json(new { isSuccess = false, message = "Only image files are allowed.(.jpg,.jpeg,.png,.gif,.webp)" });
                        }
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        // Delete old image
                        if (!string.IsNullOrEmpty(request.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(uploadsFolder, request.ImageUrl);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.ImageFile.CopyToAsync(stream);
                        }

                        request.ImageUrl = fileName;
                    }
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateProfileImage, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        var identity = new ClaimsIdentity(User.Identity);

                        var existingClaim = identity.FindFirst(ClaimTypes.GivenName);
                        if (existingClaim != null)
                        {
                            identity.RemoveClaim(existingClaim);
                        }

                        identity.AddClaim(new Claim(ClaimTypes.GivenName, request.ImageUrl));

                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(principal);
                        return Json(new { isSuccess = true, message = Response.Message, imageUrl = request.ImageUrl });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateProfile");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating profile details." });
        }
        [HttpGet]
        public async Task<IActionResult> GetUserDetails(int Id, string SPName)
        {
            List<UpdateUserDetailsRequest> result1 = new List<UpdateUserDetailsRequest>();
            try
            {
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + Id + "&SPName=" + SPName;
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    result1 = JsonConvert.DeserializeObject<List<UpdateUserDetailsRequest>>(jsonResponse);
                }
                result1.ToList().FirstOrDefault().UserName = UserName;
                result1.ToList().FirstOrDefault().UserRole = UserRole;
                string QueryString19 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=Id&SortDirection=Asc&SPName=APortal_RoleMst&Filter=";
                var response19 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryString19);
                if (response19 != null && response19.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response19.Response);
                    List<RoleModel> result30 = JsonConvert.DeserializeObject<List<RoleModel>>(jsonResponse);
                    ViewBag.Roles = new SelectList(result30, "Id", "Role");
                }
                else
                {
                    ViewBag.Roles = new SelectList(Enumerable.Empty<SelectListItem>());
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "GetUserDetails");
            }
            return PartialView("_EditUser", result1.ToList().FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProfileImage(DeleteProfileImageRequest request)
        {
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.DeleteProfileImage, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    var identity = new ClaimsIdentity(User.Identity);

                    var existingClaim = identity.FindFirst(ClaimTypes.GivenName);
                    if (existingClaim != null)
                    {
                        identity.RemoveClaim(existingClaim);
                    }

                    identity.AddClaim(new Claim(ClaimTypes.GivenName, ""));

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                    return Json(new { isSuccess = true, message = Response.Message });
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "DeleteProfileImage");
                return Json(new { isSuccess = false, message = ex.Message });
            }
        }
        #endregion

        #region User (Agent / Admin)
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Agents()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Agents");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<JsonResult> UpdateAgentsDetails(UpdateUserDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.CreateUserName = UsernameGenerator.GenerateUsername(request.FirstName, request.LastName);
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    if (request.Id == 0)
                    {
                        request.EncPassword = Encryption.Encrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), request.FirstName + "@123");
                    }
                    if (request.RoleId == 4) // If the role is "Agent", set the ReferenceCode to the user's serial number
                    {
                        request.ReferenceCode = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;
                    }
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateUsersDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateUsersDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating page details." });
        }
        public JsonResult GetCitiesByState(string state)
        {
            List<string> cities = new List<string>();

            switch (state)
            {
                case "Andhra Pradesh":
                    cities = new List<string> { "Visakhapatnam", "Vijayawada", "Guntur", "Nellore" };
                    break;

                case "Arunachal Pradesh":
                    cities = new List<string> { "Itanagar", "Tawang", "Ziro" };
                    break;

                case "Assam":
                    cities = new List<string> { "Guwahati", "Dibrugarh", "Silchar" };
                    break;

                case "Bihar":
                    cities = new List<string> { "Patna", "Gaya", "Muzaffarpur" };
                    break;

                case "Chhattisgarh":
                    cities = new List<string> { "Raipur", "Bilaspur", "Durg" };
                    break;

                case "Goa":
                    cities = new List<string> { "Panaji", "Margao", "Vasco da Gama" };
                    break;

                case "Gujarat":
                    cities = new List<string> { "Ahmedabad", "Surat", "Vadodara", "Rajkot" };
                    break;

                case "Haryana":
                    cities = new List<string> { "Gurgaon", "Faridabad", "Panipat" };
                    break;

                case "Himachal Pradesh":
                    cities = new List<string> { "Shimla", "Manali", "Dharamshala" };
                    break;

                case "Jharkhand":
                    cities = new List<string> { "Ranchi", "Jamshedpur", "Dhanbad" };
                    break;

                case "Karnataka":
                    cities = new List<string> { "Bangalore", "Mysore", "Mangalore", "Hubli" };
                    break;

                case "Kerala":
                    cities = new List<string> { "Kochi", "Thiruvananthapuram", "Kozhikode" };
                    break;

                case "Madhya Pradesh":
                    cities = new List<string> { "Bhopal", "Indore", "Gwalior", "Jabalpur" };
                    break;

                case "Maharashtra":
                    cities = new List<string> { "Mumbai", "Pune", "Nagpur", "Nashik" };
                    break;

                case "Manipur":
                    cities = new List<string> { "Imphal", "Thoubal", "Bishnupur" };
                    break;

                case "Meghalaya":
                    cities = new List<string> { "Shillong", "Tura", "Nongpoh" };
                    break;

                case "Mizoram":
                    cities = new List<string> { "Aizawl", "Lunglei", "Champhai" };
                    break;

                case "Nagaland":
                    cities = new List<string> { "Kohima", "Dimapur", "Mokokchung" };
                    break;

                case "Odisha":
                    cities = new List<string> { "Bhubaneswar", "Cuttack", "Rourkela" };
                    break;

                case "Punjab":
                    cities = new List<string> { "Ludhiana", "Amritsar", "Jalandhar" };
                    break;

                case "Rajasthan":
                    cities = new List<string> { "Jaipur", "Udaipur", "Jodhpur", "Kota" };
                    break;

                case "Sikkim":
                    cities = new List<string> { "Gangtok", "Namchi", "Geyzing" };
                    break;

                case "Tamil Nadu":
                    cities = new List<string> { "Chennai", "Coimbatore", "Madurai", "Salem", "Trichy" };
                    break;

                case "Telangana":
                    cities = new List<string> { "Hyderabad", "Warangal", "Nizamabad" };
                    break;

                case "Tripura":
                    cities = new List<string> { "Agartala", "Udaipur", "Dharmanagar" };
                    break;

                case "Uttar Pradesh":
                    cities = new List<string> { "Lucknow", "Kanpur", "Noida", "Agra", "Varanasi" };
                    break;

                case "Uttarakhand":
                    cities = new List<string> { "Dehradun", "Haridwar", "Nainital" };
                    break;

                case "West Bengal":
                    cities = new List<string> { "Kolkata", "Siliguri", "Durgapur" };
                    break;

                default:
                    cities = new List<string>();
                    break;
            }

            return Json(cities);
        }
        #endregion

        #region User (Customers)
        public async Task<IActionResult> Customers()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Customers");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateCustomersDetails(UpdateUserDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.CreateUserName = UsernameGenerator.GenerateUsername(request.FirstName, request.LastName);
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    if (request.Id == 0)
                    {
                        request.EncPassword = Encryption.Encrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), request.FirstName + "@123");
                    }
                    if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == "Power" && request.RoleId == 4) // If the role is "Agent", set the ReferenceCode to the user's serial number
                    {
                        request.ReferenceCode = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;
                    }
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateCustomersDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateCustomersDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating customer details." });
        }
        #endregion

        #region Pages
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Pages()
        {
            try
            {
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Pages");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdatePagesDetails(UpdatePageDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdatePageDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdatePagesDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating page details." });
        }
        #endregion

        #region Roles
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Roles()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Roles");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateRolesDetails(UpdateRoleDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateRoleDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateRolesDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating roles." });
        }
        #endregion

        #region Role Permissions
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> RolePermissions()
        {
            List<GetRolePermissionsRequest> result1 = new List<GetRolePermissionsRequest>();
            try
            {
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetUserLoginList + QueryString);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    result1 = JsonConvert.DeserializeObject<List<GetRolePermissionsRequest>>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "RolePermissions");
            }
            return View(result1);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> RolePermissionsChange(int id)
        {
            List<GetRolePermissionsList> result2 = new List<GetRolePermissionsList>();
            try
            {
                string QueryString1 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + id;
                var response1 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetRolePermissionsList + QueryString1);

                if (response1 != null && response1.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response1.Response);
                    result2 = JsonConvert.DeserializeObject<List<GetRolePermissionsList>>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "RolePermissions");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(result2);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> SaveRolePermissions([FromBody] UpdateUserPageRightsMappingRequest model)
        {
            try
            {
                if (model != null)
                {
                    model.UserName = UserName;
                    model.UserRole = UserRole;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateUserPageRightsMapping, model);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "An error occurred while updating roles." });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "SaveRolePermissions");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating roles." });
        }
        #endregion

        #region Bank Branch
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Branch()
        {
            try
            {
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "SystemSettings");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateBranchDetails(UpdateBranchDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateBranchDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateSystemSettings");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating system settings." });
        }
        #endregion

        #region Teams
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Teams()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Teams");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateTeams(UpdateTeamDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateTeamDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateTeams");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating team details." });
        }
        #endregion

        #region Images Configuration
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> ImageConfiguration()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ImageConfiguration");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateImageConfiguration(UpdateImageConfigDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (request.ImageFile != null && request.ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "ImageUploads");
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".jfif" };
                        var extension = Path.GetExtension(request.ImageFile.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            return Json(new { isSuccess = false, message = "Only image files are allowed.(.jpg,.jpeg,.png,.gif,.webp)" });
                        }
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        // Delete old image
                        if (!string.IsNullOrEmpty(request.ImagePath))
                        {
                            var oldImagePath = Path.Combine(uploadsFolder, request.ImagePath);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.ImageFile.CopyToAsync(stream);
                        }

                        request.ImagePath = fileName;
                    }
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateImageConfigDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateImageConfiguration");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating image configuration." });
        }
        #endregion

        #region Bank Service
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Service()
        {
            try
            {
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Service");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateServiceDetails(UpdateBankServiceDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    request.IsActive = true;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateBankServiceDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return Json(new { isSuccess = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UpdateServiceDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating service." });
        }
        #endregion

        #region Error Handling
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> ErrorHandling(GetMasterListRequest request)
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ErrorHandling");
            }
            return View();
        }
        #endregion

        #region Audit Log Handling
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> AuditLogHandling()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "AuditLogHandling");
            }
            return View();
        }
        #endregion

        #region List
        public async Task<IActionResult> List(GetMasterListRequest request)
        {
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=" + request.IsActive + "&PageNumber=" + request.PageNumber + "&PageSize=" + request.PageSize + "&PageSize=" + request.PageSize + "&PageSize=" + request.PageSize + "&SortColumn=" + request.SortColumn + "&SortDirection=" + request.SortDirection + "&SPName=" + request.SPName + "&Filter=" + request.Filter;
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryString);
                switch (request.SPName)
                {
                    case "APortal_UserManage":
                        List<GetUserDetailsModel> result1 = new List<GetUserDetailsModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result1 = JsonConvert.DeserializeObject<List<GetUserDetailsModel>>(jsonResponse);
                        }
                        return PartialView("_AgentsList", result1);
                        break;
                    case "APortal_CustomerManage":
                        List<GetUserDetailsModel> result9 = new List<GetUserDetailsModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result9 = JsonConvert.DeserializeObject<List<GetUserDetailsModel>>(jsonResponse);
                        }
                        return PartialView("_CustomersList", result9);
                        break;
                    case "APortal_PageMst":
                        List<PageModel> result2 = new List<PageModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result2 = JsonConvert.DeserializeObject<List<PageModel>>(jsonResponse);
                        }
                        return PartialView("_PagesList", result2);
                        break;
                    case "APortal_RoleMst":
                        List<RoleModel> result3 = new List<RoleModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result3 = JsonConvert.DeserializeObject<List<RoleModel>>(jsonResponse);
                        }
                        return PartialView("_RolesList", result3);
                        break;
                    case "APortal_BankBranchMst":
                        List<BankBranchModel> result4 = new List<BankBranchModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result4 = JsonConvert.DeserializeObject<List<BankBranchModel>>(jsonResponse);
                        }
                        return PartialView("_BankBranchList", result4);
                        break;
                    case "APortal_ImageConfigMst":
                        List<ImageConfigModel> result5 = new List<ImageConfigModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result5 = JsonConvert.DeserializeObject<List<ImageConfigModel>>(jsonResponse);
                        }
                        return PartialView("_ImageConfigurationList", result5);
                        break;
                    case "APortal_OrchidCapitalTeamMst":
                        List<TeamModel> result6 = new List<TeamModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result6 = JsonConvert.DeserializeObject<List<TeamModel>>(jsonResponse);
                        }
                        return PartialView("_TeamsList", result6);
                        break;
                    case "APortal_ErrorLog":
                        List<ErrorLogListModel> result7 = new List<ErrorLogListModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result7 = JsonConvert.DeserializeObject<List<ErrorLogListModel>>(jsonResponse);
                        }
                        return PartialView("_ErrorLogList", result7);
                        break;
                    case "UPortal_uspSaveAuditLog":
                        List<AuditLogListModel> result8 = new List<AuditLogListModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result8 = JsonConvert.DeserializeObject<List<AuditLogListModel>>(jsonResponse);
                        }
                        return PartialView("_AuditLogList", result8);
                        break;
                    case "APortal_LoanProductMaster":
                        List<BankServiceListModel> result10 = new List<BankServiceListModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result10 = JsonConvert.DeserializeObject<List<BankServiceListModel>>(jsonResponse);
                        }
                        return PartialView("_BankServiceList", result10);
                        break;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "List");
            }
            return PartialView("_Error", null);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create(string SPName)
        {
            try
            {
                switch (SPName)
                {
                    case "APortal_UserManage":
                        UpdateUserDetailsRequest userRequest = new UpdateUserDetailsRequest();
                        userRequest.Id = 0;
                        userRequest.IsActive = true;
                        userRequest.UserName = UserName;
                        userRequest.UserRole = UserRole;
                        string QueryString1 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=Id&SortDirection=Asc&SPName=APortal_RoleMst&Filter=";
                        var response1 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryString1);
                        if (response1 != null && response1.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response1.Response);
                            List<RoleModel> result3 = JsonConvert.DeserializeObject<List<RoleModel>>(jsonResponse);
                            ViewBag.Roles = new SelectList(result3, "Id", "Role");
                        }
                        else
                        {
                            ViewBag.Roles = new SelectList(Enumerable.Empty<SelectListItem>());
                        }
                        return PartialView("_AddAgents", userRequest);
                        break;
                    case "APortal_CustomerManage":
                        UpdateUserDetailsRequest userRequest1 = new UpdateUserDetailsRequest();
                        userRequest1.Id = 0;
                        userRequest1.IsActive = true;
                        userRequest1.UserName = UserName;
                        userRequest1.UserRole = UserRole;
                        userRequest1.ReferenceCode = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;
                        string QueryString11 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=Id&SortDirection=Asc&SPName=APortal_RoleMst&Filter=Standard";
                        var response11 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryString11);
                        if (response11 != null && response11.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response11.Response);
                            List<RoleModel> result3 = JsonConvert.DeserializeObject<List<RoleModel>>(jsonResponse);
                            ViewBag.Roles = new SelectList(result3, "Id", "Role");
                        }
                        else
                        {
                            ViewBag.Roles = new SelectList(Enumerable.Empty<SelectListItem>());
                        }
                        return PartialView("_AddCustomers", userRequest1);
                        break;
                    case "APortal_PageMst":
                        UpdatePageDetailsRequest pageRequest = new UpdatePageDetailsRequest();
                        pageRequest.UserName = UserName;
                        pageRequest.UserRole = UserRole;
                        pageRequest.Id = 0;
                        pageRequest.IsActive = true;
                        string QueryStringMenu = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                        var responsemenu = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMenuList + QueryStringMenu);
                        if (responsemenu != null && responsemenu.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(responsemenu.Response);
                            List<MenuModel> resultmenu = JsonConvert.DeserializeObject<List<MenuModel>>(jsonResponse);
                            ViewBag.MenuList = new SelectList(resultmenu, "Id", "Name");
                        }
                        else
                        {
                            ViewBag.MenuList = new SelectList(Enumerable.Empty<SelectListItem>());
                        }
                        return PartialView("_AddPages", pageRequest);
                        break;
                    case "APortal_RoleMst":
                        UpdateRoleDetailsRequest roleRequest = new UpdateRoleDetailsRequest();
                        roleRequest.UserName = UserName;
                        roleRequest.UserRole = UserRole;
                        roleRequest.Id = 0;
                        roleRequest.IsActive = true;
                        return PartialView("_AddRoles", roleRequest);
                        break;
                    case "APortal_BankBranchMst":
                        UpdateBranchDetailsRequest configSettingsRequest = new UpdateBranchDetailsRequest();
                        configSettingsRequest.UserName = UserName;
                        configSettingsRequest.UserRole = UserRole;
                        configSettingsRequest.BranchId = 0;
                        configSettingsRequest.IsActive = true;
                        return PartialView("_AddBankBranch", configSettingsRequest);
                        break;
                    case "APortal_ImageConfigMst":
                        UpdateImageConfigDetailsRequest imageConfigRequest = new UpdateImageConfigDetailsRequest();
                        imageConfigRequest.UserName = UserName;
                        imageConfigRequest.UserRole = UserRole;
                        imageConfigRequest.Id = 0;
                        imageConfigRequest.IsActive = true;
                        return PartialView("_AddImageConfiguration", imageConfigRequest);
                        break;
                    case "APortal_OrchidCapitalTeamMst":
                        UpdateTeamDetailsRequest teamRequest = new UpdateTeamDetailsRequest();
                        teamRequest.UserName = UserName;
                        teamRequest.UserRole = UserRole;
                        teamRequest.Id = 0;
                        teamRequest.IsActive = true;
                        return PartialView("_AddTeams", teamRequest);
                        break;
                    case "APortal_UserPageRightsMappingMst":
                        string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                        var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetUserLoginList + QueryString);
                        List<GetRolePermissionsRequest> result1 = new List<GetRolePermissionsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result1 = JsonConvert.DeserializeObject<List<GetRolePermissionsRequest>>(jsonResponse);
                        }
                        AddRolePermissionsRequest rightsRequest = new AddRolePermissionsRequest();
                        rightsRequest.UserName = UserName;
                        rightsRequest.UserRole = UserRole;
                        rightsRequest.RolePermissions = result1;
                        return PartialView("_AddRolePermissions", rightsRequest);
                        break;
                    case "APortal_LoanProductMaster":
                        UpdateBankServiceDetailsRequest service = new UpdateBankServiceDetailsRequest();
                        service.UserName = UserName;
                        service.UserRole = UserRole;
                        service.IsActive = true;
                        return PartialView("_AddBankService", service);
                        break;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Create");
            }
            return PartialView("_Error", null);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int Id, string SPName)
        {
            try
            {
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + Id + "&SPName=" + SPName;
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString);
                switch (SPName)
                {
                    case "APortal_UserManage":
                        List<UpdateUserDetailsRequest> result1 = new List<UpdateUserDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result1 = JsonConvert.DeserializeObject<List<UpdateUserDetailsRequest>>(jsonResponse);
                        }
                        result1.ToList().FirstOrDefault().UserName = UserName;
                        result1.ToList().FirstOrDefault().UserRole = UserRole;
                        string QueryString19 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=Id&SortDirection=Asc&SPName=APortal_RoleMst&Filter=";
                        var response19 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryString19);
                        if (response19 != null && response19.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response19.Response);
                            List<RoleModel> result30 = JsonConvert.DeserializeObject<List<RoleModel>>(jsonResponse);
                            ViewBag.Roles = new SelectList(result30, "Id", "Role");
                        }
                        else
                        {
                            ViewBag.Roles = new SelectList(Enumerable.Empty<SelectListItem>());
                        }
                        return PartialView("_AddAgents", result1.ToList().FirstOrDefault());
                        break;
                    case "APortal_CustomerManage":
                        List<UpdateUserDetailsRequest> result10 = new List<UpdateUserDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result10 = JsonConvert.DeserializeObject<List<UpdateUserDetailsRequest>>(jsonResponse);
                        }
                        result10.ToList().FirstOrDefault().UserName = UserName;
                        result10.ToList().FirstOrDefault().UserRole = UserRole;
                        string QueryString190 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=1&PageNumber=0&PageSize=" + int.MaxValue + "&SortColumn=Id&SortDirection=Asc&SPName=APortal_RoleMst&Filter=Standard";
                        var response190 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryString190);
                        if (response190 != null && response190.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response190.Response);
                            List<RoleModel> result30 = JsonConvert.DeserializeObject<List<RoleModel>>(jsonResponse);
                            ViewBag.Roles = new SelectList(result30, "Id", "Role");
                        }
                        else
                        {
                            ViewBag.Roles = new SelectList(Enumerable.Empty<SelectListItem>());
                        }
                        return PartialView("_AddCustomers", result10.ToList().FirstOrDefault());
                        break;
                    case "APortal_PageMst":
                        List<UpdatePageDetailsRequest> result2 = new List<UpdatePageDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result2 = JsonConvert.DeserializeObject<List<UpdatePageDetailsRequest>>(jsonResponse);
                        }
                        result2.ToList().FirstOrDefault().UserName = UserName;
                        result2.ToList().FirstOrDefault().UserRole = UserRole;
                        string QueryStringMenu = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                        var responsemenu = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMenuList + QueryStringMenu);
                        if (responsemenu != null && responsemenu.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(responsemenu.Response);
                            List<MenuModel> resultmenu = JsonConvert.DeserializeObject<List<MenuModel>>(jsonResponse);
                            ViewBag.MenuList = new SelectList(resultmenu, "Id", "Name");
                        }
                        else
                        {
                            ViewBag.MenuList = new SelectList(Enumerable.Empty<SelectListItem>());
                        }
                        return PartialView("_AddPages", result2.ToList().FirstOrDefault());
                        break;
                    case "APortal_RoleMst":
                        List<UpdateRoleDetailsRequest> result3 = new List<UpdateRoleDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result3 = JsonConvert.DeserializeObject<List<UpdateRoleDetailsRequest>>(jsonResponse);
                        }
                        result3.ToList().FirstOrDefault().UserName = UserName;
                        result3.ToList().FirstOrDefault().UserRole = UserRole;
                        return PartialView("_AddRoles", result3.ToList().FirstOrDefault());
                        break;
                    case "APortal_BankBranchMst":
                        List<UpdateBranchDetailsRequest> result4 = new List<UpdateBranchDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result4 = JsonConvert.DeserializeObject<List<UpdateBranchDetailsRequest>>(jsonResponse);
                        }
                        result4.ToList().FirstOrDefault().UserName = UserName;
                        result4.ToList().FirstOrDefault().UserRole = UserRole;
                        return PartialView("_AddBankBranch", result4.ToList().FirstOrDefault());
                        break;
                    case "APortal_ImageConfigMst":
                        List<UpdateImageConfigDetailsRequest> result5 = new List<UpdateImageConfigDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result5 = JsonConvert.DeserializeObject<List<UpdateImageConfigDetailsRequest>>(jsonResponse);
                        }
                        result5.ToList().FirstOrDefault().UserName = UserName;
                        result5.ToList().FirstOrDefault().UserRole = UserRole;
                        return PartialView("_AddImageConfiguration", result5.ToList().FirstOrDefault());
                        break;
                    case "APortal_OrchidCapitalTeamMst":
                        List<UpdateTeamDetailsRequest> result6 = new List<UpdateTeamDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result6 = JsonConvert.DeserializeObject<List<UpdateTeamDetailsRequest>>(jsonResponse);
                        }
                        result6.ToList().FirstOrDefault().UserName = UserName;
                        result6.ToList().FirstOrDefault().UserRole = UserRole;
                        return PartialView("_AddTeams", result6.ToList().FirstOrDefault());
                        break;
                    case "APortal_UserPageRightsMappingMst":
                        string QueryString1 = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                        var response1 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetUserLoginList + QueryString1);
                        List<GetRolePermissionsRequest> result7 = new List<GetRolePermissionsRequest>();
                        if (response1 != null && response1.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response1.Response);
                            result7 = JsonConvert.DeserializeObject<List<GetRolePermissionsRequest>>(jsonResponse);
                        }
                        AddRolePermissionsRequest rightsRequest = new AddRolePermissionsRequest();
                        rightsRequest.UserName = UserName;
                        rightsRequest.UserRole = UserRole;
                        rightsRequest.UserId = Id;
                        rightsRequest.RolePermissions = result7;
                        return PartialView("_AddRolePermissions", rightsRequest);
                        break;
                    case "APortal_LoanProductMaster":
                        List<UpdateBankServiceDetailsRequest> service = new List<UpdateBankServiceDetailsRequest>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            service = JsonConvert.DeserializeObject<List<UpdateBankServiceDetailsRequest>>(jsonResponse);
                        }
                        service.ToList().FirstOrDefault().UserName = UserName;
                        service.ToList().FirstOrDefault().UserRole = UserRole;
                        return PartialView("_AddBankService", service.ToList().FirstOrDefault());
                        break;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Edit");
            }
            return View();
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(DeleteMasterDetailsRequest request)
        {
            string message = string.Empty;
            bool isSuccess = false;
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.DeleteMasterDetails, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    message = Response.Message;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Delete");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { message = message, isSuccess = isSuccess });
        }
        #endregion

        #region Status Change
        public async Task<IActionResult> Status(ChangeStatusInMasterDetailsRequest request)
        {
            string message = string.Empty;
            bool isSuccess = false;
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.ChangeStatusInMasterDetails, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    message = Response.Message;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "Status");
                return Json(new { message = ex.Message, isSuccess = false });
            }
            return Json(new { message = message, isSuccess = isSuccess });
        }
        #endregion

        #region Export to Excel
        public async Task<IActionResult> ExportToExcelFile(GetMasterListRequest request)
        {
            try
            {
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&IsActive=" + request.IsActive + "&PageNumber=" + 0 + "&PageSize=" + int.MaxValue + "&SortColumn=" + request.SortColumn + "&SortDirection=" + request.SortDirection + "&SPName=" + request.SPName + "&Filter=" + request.Filter;
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetMasterList + QueryString);
                switch (request.SPName)
                {
                    case "APortal_UserManage":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "User List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_CustomerManage":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Customer List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_PageMst":
                        List<PageModel> result2 = new List<PageModel>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Page List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_RoleMst":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Role List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_BankBranchMst":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Config Settings List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_ImageConfigMst":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Image Config List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_OrchidCapitalTeamMst":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Team Members List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_ErrorLog":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Error Log List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "UPortal_uspSaveAuditLog":
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Audit Log List",
                                data = result,
                                ExcludeColumnList = "Id,Icon,TotalRecords",
                                filters = new Dictionary<string, string>() { { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                    case "APortal_LoanProductMaster":
                         if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            DataTable result = JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                            ExportDataModel<DataTable> exportDataModel = new ExportDataModel<DataTable>()
                            {
                                Title = "Bank Service List",
                                data = result,
                                ExcludeColumnList = "LoanProductId,TotalRecords",
                                filters = new Dictionary<string, string>() { { "IsActive", request.IsActive == 1 ? "Active" : request.IsActive == 0 ? "Inactive" : "All" }, { "Filter", request.Filter } }
                            };
                            string errorMsg = string.Empty;
                            byte[] fileBytes = ExportToExcel.GenerateExcel(exportDataModel, ref errorMsg);
                            if (fileBytes != null)
                            {
                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", exportDataModel.Title + ".xlsx");
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "An error occurred while generating the Excel file: " + errorMsg });
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ExportToExcel");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while fetching data for export." });
        }
        #endregion

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

        #region Change Password
        public async Task<IActionResult> ChangePassword()
        {
            ChangePasswordViewModel request = new ChangePasswordViewModel();
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                request.PublicKey = _rsaService.GetPublicKey();
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ResetPassword");
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
                        string QueryString = $"?UserName={UserName}";
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

                        request.UserName = UserName;
                        request.UserRole = UserRole;
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
                    return RedirectToAction("ChangePassword", "Admin");
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ChangePassword");
            }
            return RedirectToAction("ChangePassword", "Admin");
        }
        #endregion

        #region Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Forgot Password POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.ForgotPassword, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ForgotPassword", "Admin");
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    string errorMessage = string.Join("; ", errors);
                    return RedirectToAction("ForgotPassword", "Admin");
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ForgotPassword");
            }
            return RedirectToAction("ForgotPassword", "Admin");
        }
        #endregion

        #region Reset Password
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            string message = string.Empty;
            try
            {
                string Query = @"?ResetCode=" + token;
                var responce = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.VerifyResetPasswordRequest + Query);
                if (responce != null && responce.IsSuccessStatusCode)
                {
                    message = "";
                }
                else
                {
                    message = "Reset password link has expired. Please request a new one. (Reset password request is valid for 24 hours only)";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                await SaveErrorLog(ex, "AdminController", "ResetPassword");
            }
            var model = new ResetPasswordViewModel
            {
                ResetCode = token,
                PublicKey = _rsaService.GetPublicKey(),
                ErrorMessage = message
            };
            return View(model);
        }

        // Reset Password POST
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(Request.Form["hdnEncryptedNewPassword"].ToString()))
                    {
                        request.NewPassword = _rsaService.Decrypt(Request.Form["hdnEncryptedNewPassword"].ToString());
                    }
                    if (!request.NewPassword.StartsWith("*******"))
                    {
                        // Check Password (Current Passswor & Database Password)
                        string QueryString = $"?UserName={UserName}";
                        var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetUserPassword + QueryString);
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string encryptedPassword = response.Response[0].EncPassword.ToString();
                            string decryptedPassword = Encryption.Decrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), encryptedPassword);
                            if (request.NewPassword != decryptedPassword)
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
                        request.NewPassword = Encryption.Encrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"), request.NewPassword);
                        var response1 = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.ForgotPassword, request);
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
                    return RedirectToAction("ResetPassword", "Admin");
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ResetPassword");
            }
            return RedirectToAction("ResetPassword", "Admin");
        }
        #endregion

        #region Two Factor Authentication | Generate QR Code | Verify QR Code | Disable Two Factor Authentication
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "TwoFactorAuthentication");
            }
            return PartialView("_TwoFactor");
        }
        [HttpGet]
        public async Task<IActionResult> GenerateQRCode()
        {
            string QRCodeUrl = string.Empty;
            try
            {
                var tfa = new TwoFactorAuth(UserName);
                var SecretKey = tfa.CreateSecret(160);
                _httpContextAccessor.HttpContext.Session.SetString("SecretKey", SecretKey);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QRCodeUrl = tfa.GetQrCodeImageAsDataUri("Orchid Capital", SecretKey);
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "GenerateQRCode");
            }
            return Json(new { QRCodeUrl = QRCodeUrl });
        }
        [HttpPost]
        public async Task<IActionResult> TwoFactorAuthentication(TwoFactorRequest request)
        {
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                request.IsTwoFactor = true;
                request.SecretKey = _httpContextAccessor.HttpContext.Session.GetString("SecretKey");
                var tfa = new TwoFactorAuth();
                bool retValue = tfa.VerifyCode(request.SecretKey, request.AuthCode);
                if (retValue)
                {
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.TwoFactorAuthentication, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "TwoFactorAuthentication");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while processing your request." });
        }

        public async Task<IActionResult> DisableTwoFactor()
        {
            try
            {
                TwoFactorRequest request = new TwoFactorRequest();
                request.UserName = UserName;
                request.UserRole = UserRole;
                request.SecretKey = "";
                request.IsTwoFactor = false;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.TwoFactorAuthentication, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    return Json(new { isSuccess = true, message = Response.Message });
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "DisableTwoFactor");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while processing your request." });
        }
        #endregion

        #region Send welcome email | User unlock |User reset password |User reset two factor authentication | Delete User
        [HttpPost]
        public async Task<IActionResult> SendWelcomeEmailUser(SendWelcomeMailRequest request)
        {
            bool success = false; string Message = string.Empty;
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.SendWelcomeMail, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    success = true; Message = Response.Message;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "SendWelcomeEmail");
                success = false; Message = ex.Message;
            }
            return Json(new { isSuccess = success, message = Message });
        }
        [HttpPost]
        public async Task<IActionResult> UnlockUser(SendWelcomeMailRequest request)
        {
            bool success = false; string Message = string.Empty;
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UnlockUser, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    success = true; Message = Response.Message;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "UnlockUser");
                success = false; Message = ex.Message;
            }
            return Json(new { isSuccess = success, message = Message });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordUser(SendWelcomeMailRequest request)
        {
            bool success = false; string Message = string.Empty;
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.ResetPasswordUser, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    ResetPasswordUserModel Response = JsonConvert.DeserializeObject<List<ResetPasswordUserModel>>(jsonResponse)[0];
                    success = true; Message = response.Message;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ResetPasswordUser");
                success = false; Message = ex.Message;
            }
            return Json(new { isSuccess = success, message = Message });
        }
        [HttpPost]
        public async Task<IActionResult> ResetTwoFactorAuthenticationUser(SendWelcomeMailRequest request)
        {
            bool success = false; string Message = string.Empty;
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.ResetTwoFactorAuthentication, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    success = true; Message = Response.Message;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "ResetTwoFactorAuthenticationUser");
                success = false; Message = ex.Message;
            }
            return Json(new { isSuccess = success, message = Message });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(SendWelcomeMailRequest request)
        {
            bool success = false; string Message = string.Empty;
            try
            {
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.DeleteUser, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    CommonResponseModel Response = JsonConvert.DeserializeObject<List<CommonResponseModel>>(jsonResponse)[0];
                    success = true; Message = Response.Message;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "AdminController", "SendWelcomeEmail");
                success = false; Message = ex.Message;
            }
            return Json(new { isSuccess = success, message = Message });
        }
        #endregion
    }
}