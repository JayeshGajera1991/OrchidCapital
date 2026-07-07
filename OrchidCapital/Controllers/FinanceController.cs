using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrchidCapital.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchidCapital.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
namespace OrchidCapital.Controllers
{
    [SessionIsRepeat]
    [SessionTimeOut]
    public class FinanceController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly RsaService _rsaService;
        private readonly IWebHostEnvironment _environment;
        public FinanceController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, RsaService rsaService, IWebHostEnvironment environment) : base(httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _rsaService = rsaService;
            _environment = environment;
        }

        #region New Loan Application
        [Authorize(Roles = "Admin,SuperAdmin,Power")]
        public async Task<IActionResult> NewLoanApplication(string LoanApplicationId = "", string LoanStep = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(LoanApplicationId))
                {
                    ViewBag.LoanApplicationId = Encryption.DecryptParameter(LoanApplicationId);
                    ViewBag.LoanStep = Encryption.DecryptParameter(LoanStep);
                }
                if (string.IsNullOrEmpty(LoanApplicationId))
                {
                    ViewBag.LoanApplicationId = 0;
                    ViewBag.LoanStep = 0;
                }

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceLoanApplication", "NewLoanApplication");
            }
            return View();
        }

        private async Task<List<CommonListModel>> GetCommonList(string option)
        {
            List<CommonListModel> result = new List<CommonListModel>();
            try
            {
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Option=" + option;
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetCommonList + QueryString);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    result = JsonConvert.DeserializeObject<List<CommonListModel>>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "GetCommonList");
            }
            return result;
        }

        public async Task<IActionResult> LoanApplicationProcessStep(int id, int loanApplicationId)
        {
            try
            {
                switch (id)
                {
                    case 1:
                        UpdateLoanApplicationDocumentRequest model1 = new UpdateLoanApplicationDocumentRequest();
                        model1 = await BindLoanApplicationDocument(loanApplicationId);
                        return PartialView("_AddLoanDocument", model1);
                    case 2:
                        UpdateLoanApplicationGuarantorRequest model2 = new UpdateLoanApplicationGuarantorRequest();
                        model2 = await BindGuarantor(loanApplicationId);
                        return PartialView("_AddLoanGuarantorInformation", model2);
                    case 3:
                        LoanApplicationReview loanApplication = new LoanApplicationReview();
                        loanApplication = await BindLoanApplicationReview(loanApplicationId);
                        return PartialView("_AddLoanApplicationReview", loanApplication);
                    default:
                        List<UpdateLoanApplicationDetailsRequest> model = new List<UpdateLoanApplicationDetailsRequest>();
                        model = await BindLoanApplicationDetails(loanApplicationId);
                        return PartialView("_AddLoanApplicationDetails", model.ToList().FirstOrDefault());
                        break;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "LoanApplicationProcessStep");
            }
            return PartialView("_Error", null);
        }

        private async Task<LoanApplicationReview> BindLoanApplicationReview(int loanApplicationId)
        {
            LoanApplicationReview review = new LoanApplicationReview();
            try
            {
                string QueryString3 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + loanApplicationId + "&SPName=APortal_LoanApplication";
                var response3 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString3);
                if (response3 != null && response3.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response3.Response);
                    List<GetLoanApplicationList> model = JsonConvert.DeserializeObject<List<GetLoanApplicationList>>(jsonResponse);
                    if (model != null && model.Count > 0)
                    {
                        review.getLoanDetails = model.ToList().FirstOrDefault();
                    }
                }


                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + loanApplicationId + "&SPName=APortal_LoanDocuments";
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    List<GetLoanApplicationDocumentList> model1 = JsonConvert.DeserializeObject<List<GetLoanApplicationDocumentList>>(jsonResponse);
                    if (model1 != null && model1.Count > 0)
                    {
                        review.documentLists = model1.ToList();
                    }
                }


                string QueryString1 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + loanApplicationId + "&SPName=APortal_LoanGuarantor";
                var response1 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString1);
                if (response1 != null && response1.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response1.Response);
                    List<UpdateGuarantorRequest> model5 = JsonConvert.DeserializeObject<List<UpdateGuarantorRequest>>(jsonResponse);
                    if (model5 != null && model5.Count > 0)
                    {
                        review.guarantorRequests = model5.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "BindLoanApplicationReview");
            }
            return review;
        }

        private async Task<List<UpdateLoanApplicationDetailsRequest>> BindLoanApplicationDetails(int loanApplicationId)
        {
            List<UpdateLoanApplicationDetailsRequest> model = new List<UpdateLoanApplicationDetailsRequest>();
            try
            {
                string QueryString3 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + loanApplicationId + "&SPName=APortal_LoanApplication";
                var response3 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString3);
                if (response3 != null && response3.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response3.Response);
                    model = JsonConvert.DeserializeObject<List<UpdateLoanApplicationDetailsRequest>>(jsonResponse);
                }
                if (model != null && model.Count > 0)
                {
                    ViewBag.GetNewApplicationNumber = model.ToList().FirstOrDefault().ApplicationNo;
                }
                else
                {
                    List<GetNewApplicationNumber> model1 = new List<GetNewApplicationNumber>();
                    string QueryString1 = @"?UserName=" + UserName + "&UserRole=" + UserRole;
                    var response1 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetNewApplicationNumber + QueryString1);
                    if (response1 != null && response1.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response1.Response);
                        model1 = JsonConvert.DeserializeObject<List<GetNewApplicationNumber>>(jsonResponse);
                    }
                    if (model1 != null && model1.Count > 0)
                    {
                        ViewBag.GetNewApplicationNumber = model1.ToList().FirstOrDefault().ApplicationNumber;
                    }
                    else
                    {
                        ViewBag.GetNewApplicationNumber = "OC000000";
                    }
                }
                ViewBag.LoanProductList = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.CustomerList = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.BranchList = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ApplicationStatusList = new SelectList(Enumerable.Empty<SelectListItem>());
                List<CommonListModel> result = await GetCommonList("LoanProductList");
                if (result != null && result.Count > 0)
                {
                    ViewBag.LoanProductList = new SelectList(result, "Id", "Name");
                }
                List<CommonListModel> result1 = await GetCommonList("CustomerList");
                if (result1 != null && result1.Count > 0)
                {
                    ViewBag.CustomerList = new SelectList(result1, "Id", "Name");
                }
                List<CommonListModel> result2 = await GetCommonList("BranchList");
                if (result2 != null && result2.Count > 0)
                {
                    ViewBag.BranchList = new SelectList(result2, "Id", "Name");
                }
                List<CommonListModel> result3 = await GetCommonList("ApplicationStatusList");
                if (result3 != null && result3.Count > 0)
                {
                    ViewBag.ApplicationStatusList = new SelectList(result3, "Id", "Name");
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceLoanApplication", "BindLoanApplicationDetails");
            }
            return model;
        }

        private async Task<UpdateLoanApplicationDocumentRequest> BindLoanApplicationDocument(int loanApplicationId)

        {
            UpdateLoanApplicationDocumentRequest documentRequest = new UpdateLoanApplicationDocumentRequest();
            try
            {
                List<LoanApplicationDocumentViewModel> model1 = new List<LoanApplicationDocumentViewModel>();
                string QueryString = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + loanApplicationId + "&SPName=APortal_LoanDocuments";
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    model1 = JsonConvert.DeserializeObject<List<LoanApplicationDocumentViewModel>>(jsonResponse);
                    documentRequest = new UpdateLoanApplicationDocumentRequest
                    {
                        LoanApplicationId = loanApplicationId,
                        Documents = model1
                    };
                }
                else
                {
                    documentRequest = new UpdateLoanApplicationDocumentRequest
                    {
                        LoanApplicationId = loanApplicationId,
                        Documents = new List<LoanApplicationDocumentViewModel>
                                {
                                    new(){ DocumentType="AADHAAR", DocumentName="Aadhaar Card"},
                                    new(){ DocumentType="PAN", DocumentName="PAN Card"},
                                    new(){ DocumentType="PHOTO", DocumentName="Photograph"},
                                    new(){ DocumentType="ADDRESS", DocumentName="Address Proof"},
                                    new(){ DocumentType="SALARY", DocumentName="Salary Slip"},
                                    new(){ DocumentType="INCOMETAX", DocumentName="Income Tax Return"},
                                    new(){ DocumentType="BANK", DocumentName="Bank Statement"},
                                    new(){ DocumentType="PROPERTY", DocumentName="Property Documents"}
                                }
                    };
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceLoanApplication", "BindLoanApplicationDocument");
            }
            return documentRequest;
        }

        private async Task<UpdateLoanApplicationGuarantorRequest> BindGuarantor(int loanApplicationId)
        {
            UpdateLoanApplicationGuarantorRequest model2 = new UpdateLoanApplicationGuarantorRequest();
            try
            {
                List<GetLoanApplicationList> model = new List<GetLoanApplicationList>();
                string QueryString3 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + loanApplicationId + "&SPName=APortal_LoanApplication";
                var response3 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString3);
                if (response3 != null && response3.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response3.Response);
                    model = JsonConvert.DeserializeObject<List<GetLoanApplicationList>>(jsonResponse);
                }
                string QueryString1 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + loanApplicationId + "&SPName=APortal_LoanGuarantor";
                var response1 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString1);
                if (response1 != null && response1.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response1.Response);
                    List<GuarantorViewModel> model5 = JsonConvert.DeserializeObject<List<GuarantorViewModel>>(jsonResponse);
                    foreach (var item in model5)
                    {
                        List<GuarantorDocumentViewModel> model6 = new List<GuarantorDocumentViewModel>();
                        string QueryString2 = @"?UserName=" + UserName + "&UserRole=" + UserRole + "&Id=" + item.GuarantorId + "&SPName=APortal_LoanGuarantorDocument";
                        var response2 = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.EditMasterDetails + QueryString2);
                        if (response2 != null && response2.IsSuccessStatusCode)
                        {
                            string jsonResponse1 = JsonConvert.SerializeObject(response2.Response);
                            model6 = JsonConvert.DeserializeObject<List<GuarantorDocumentViewModel>>(jsonResponse1);
                        }
                        model2.Guarantors.Add(new GuarantorViewModel
                        {
                            GuarantorId = item.GuarantorId,
                            LoanApplicationId = item.LoanApplicationId,
                            Name = item.Name,
                            Mobile = item.Mobile,
                            Occupation = item.Occupation,
                            Relationship = item.Relationship,
                            PANNumber = item.PANNumber,
                            AadhaarNumber = item.AadhaarNumber,
                            Address = item.Address,
                            Documents = model6
                        });
                        if (model != null && model.Count > 0)
                        {
                            model2.ApplicationNo = model.ToList().FirstOrDefault().ApplicationNo;
                            model2.BranchName = model.ToList().FirstOrDefault().BranchName;
                            model2.CustomerName = model.ToList().FirstOrDefault().CustomerName;
                            model2.RequestedAmount = model.ToList().FirstOrDefault().RequestedAmount;
                        }
                        model2.LoanApplicationId = loanApplicationId;
                    }
                }
                else
                {
                    model2.Guarantors.Add(new GuarantorViewModel
                    {
                        LoanApplicationId = loanApplicationId,
                        Documents = new List<GuarantorDocumentViewModel>
                            {
                                new(){ DocumentName="PAN Card"},
                                new(){ DocumentName="Aadhaar Card"}
                            }
                    });
                    if (model != null && model.Count > 0)
                    {
                        model2.ApplicationNo = model.ToList().FirstOrDefault().ApplicationNo;
                        model2.BranchName = model.ToList().FirstOrDefault().BranchName;
                        model2.CustomerName = model.ToList().FirstOrDefault().CustomerName;
                        model2.RequestedAmount = model.ToList().FirstOrDefault().RequestedAmount;
                    }
                    model2.LoanApplicationId = loanApplicationId;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceLoanApplication", "BindGuarantor");
            }
            return model2;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin,Power")]
        public async Task<IActionResult> UpdateLoanApplicationDetails(UpdateLoanApplicationDetailsRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.UserName = UserName;
                    request.UserRole = UserRole;
                    var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateLoanApplicationDetails, request);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(response.Response);
                        LoadCommonResponseModel Response = JsonConvert.DeserializeObject<List<LoadCommonResponseModel>>(jsonResponse)[0];
                        return Json(new { isSuccess = true, message = Response.Message, id = Response.LoanApplicationId });
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
                await SaveErrorLog(ex, "FinanceController", "UpdateLoanApplicationDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = false, message = "An error occurred while updating loan application details." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin,Power")]
        public async Task<IActionResult> UpdateLoanApplicationDocumentDetails(UpdateLoanApplicationDocumentRequest model)
        {
            LoadCommonResponseModel Response = new LoadCommonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model.Documents)
                    {
                        if (item.ImageFile != null)
                        {
                            UpdareLoanApplicationDocumentDetails request = new UpdareLoanApplicationDocumentDetails();
                            request.UserName = UserName;
                            request.UserRole = UserRole;
                            request.DocumentId = item.DocumentId;
                            request.LoanApplicationId = model.LoanApplicationId;
                            request.DocumentType = item.DocumentType;
                            request.DocumentName = item.DocumentName;
                            request.FilePath = item.FilePath;
                            var fileName = Guid.NewGuid() + Path.GetExtension(item.ImageFile.FileName);
                            var folder = Path.Combine(_environment.WebRootPath, "loandocuments");
                            if (!Directory.Exists(folder))
                                Directory.CreateDirectory(folder);
                            var filePath = Path.Combine(folder, fileName);

                            // Delete old image
                            if (!string.IsNullOrEmpty(request.FilePath))
                            {
                                var oldImagePath = Path.Combine(folder, request.FilePath);

                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await item.ImageFile.CopyToAsync(stream);
                            }
                            request.FilePath = fileName;

                            var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateLoanDocumentDetails, request);
                            if (response != null && response.IsSuccessStatusCode)
                            {
                                string jsonResponse = JsonConvert.SerializeObject(response.Response);
                                Response = JsonConvert.DeserializeObject<List<LoadCommonResponseModel>>(jsonResponse)[0];
                            }
                        }
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
                await SaveErrorLog(ex, "FinanceController", "UpdateLoanApplicationDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = true, message = Response.Message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin,Power")]
        public async Task<IActionResult> UpdateLoanApplicationGuarantorDetails(UpdateLoanApplicationGuarantorRequest model)
        {
            LoadCommonResponseModel Response = new LoadCommonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var guarantor in model.Guarantors)
                    {
                        UpdateGuarantorRequest request = new UpdateGuarantorRequest();
                        request.UserName = UserName;
                        request.UserRole = UserRole;
                        request.GuarantorId = guarantor.GuarantorId;
                        request.LoanApplicationId = model.LoanApplicationId;
                        request.Name = guarantor.Name;
                        request.Mobile = guarantor.Mobile;
                        request.Occupation = guarantor.Occupation;
                        request.Relationship = guarantor.Relationship;
                        request.PANNumber = guarantor.PANNumber;
                        request.AadhaarNumber = guarantor.AadhaarNumber;
                        request.Address = guarantor.Address;
                        Response = new LoadCommonResponseModel();
                        var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateLoanGuarantorDetails, request);
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            Response = JsonConvert.DeserializeObject<List<LoadCommonResponseModel>>(jsonResponse)[0];
                        }

                        foreach (var doc in guarantor.Documents)
                        {
                            if (doc.ImageFile != null)
                            {
                                GuarantorDocumentViewModel request1 = new GuarantorDocumentViewModel();
                                request1.UserName = UserName;
                                request1.UserRole = UserRole;
                                request1.GuarantorDocumentId = doc.GuarantorDocumentId;
                                request1.GuarantorId = Response.Id;
                                request1.DocumentName = doc.DocumentName;
                                request1.FilePath = doc.FilePath;
                                // Upload file
                                var fileName = Guid.NewGuid() + Path.GetExtension(doc.ImageFile.FileName);
                                var folder = Path.Combine(_environment.WebRootPath, "loanguarantors");
                                if (!Directory.Exists(folder))
                                    Directory.CreateDirectory(folder);

                                var filePath = Path.Combine(folder, fileName);
                                // Delete old image
                                if (!string.IsNullOrEmpty(request1.FilePath))
                                {
                                    var oldImagePath = Path.Combine(folder, request1.FilePath);

                                    if (System.IO.File.Exists(oldImagePath))
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                }

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await doc.ImageFile.CopyToAsync(stream);
                                }
                                request1.FilePath = fileName;
                                var response1 = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateLoanGuarantorDocumentDetails, request1);
                                if (response1 != null && response1.IsSuccessStatusCode)
                                {
                                    string jsonResponse = JsonConvert.SerializeObject(response1.Response);
                                    LoadCommonResponseModel Response1 = JsonConvert.DeserializeObject<List<LoadCommonResponseModel>>(jsonResponse)[0];
                                }
                            }
                        }
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
                await SaveErrorLog(ex, "FinanceController", "UpdateLoanApplicationDetails");
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return Json(new { isSuccess = true, message = Response.Message });
        }

        public async Task<IActionResult> UpdateFinalLoanApplicationDetails(int LoanApplicationId)
        {
            LoadCommonResponseModel Response = new LoadCommonResponseModel();
            try
            {
                UpdateFinalLoanApplicationDetails request = new UpdateFinalLoanApplicationDetails();
                request.LoanApplicationId = LoanApplicationId;
                request.UserName = UserName;
                request.UserRole = UserRole;
                var response = await _OrchidClient.PostAsync<dynamic>(ProxyAPI.UpdateFinalLoanApplicationDetails, request);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string jsonResponse = JsonConvert.SerializeObject(response.Response);
                    Response = JsonConvert.DeserializeObject<List<LoadCommonResponseModel>>(jsonResponse)[0];
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "UpdateFinalLoanApplicationDetails");
            }
            return Json(new { isSuccess = true, message = Response.Message });
        }
        #endregion

        #region Loan Application List
        public async Task<IActionResult> LoanApplicationList()
        {
            try
            {
                ViewBag.ApplicationStatusList = new SelectList(Enumerable.Empty<SelectListItem>());
                List<CommonListModel> result3 = await GetCommonList("ApplicationStatusList");
                if (result3 != null && result3.Count > 0)
                {
                    ViewBag.ApplicationStatusList = new SelectList(result3, "Id", "Name");
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceLoanApplication", "LoanApplicationList");
            }
            return View();
        }
        #endregion

        #region Loan Approval List
        public async Task<IActionResult> LoanApprovalList(string LoanApplicationId = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(LoanApplicationId))
                {
                    ViewBag.LoanApplicationId = Encryption.DecryptParameter(LoanApplicationId);
                    //ViewBag.LoanStep = Encryption.DecryptParameter(LoanStep);
                }
                if (string.IsNullOrEmpty(LoanApplicationId))
                {
                    ViewBag.LoanApplicationId = 0;
                    //ViewBag.LoanStep = 0;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "ApplicationReport");
            }
            return View();
        }
        #endregion

        #region Loan Approval
        public async Task<IActionResult> LoanApproval()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "ApplicationReport");
            }
            return View();
        }
        #endregion

        #region Daily Report
        public async Task<IActionResult> DailyReport()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "DailyReport");
            }
            return View();
        }
        #endregion

        #region Financial Report
        public async Task<IActionResult> FinancialReport()
        {
            try
            {

            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "FinancialReport");
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
                var response = await _OrchidClient.GetAsync<dynamic>(ProxyAPI.GetFinanceList + QueryString);
                switch (request.SPName)
                {
                    case "APortal_LoanApplication":
                        List<GetLoanApplicationList> result1 = new List<GetLoanApplicationList>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result1 = JsonConvert.DeserializeObject<List<GetLoanApplicationList>>(jsonResponse);
                        }
                        return PartialView("_LoanApplicationList", result1);
                        break;

                    case "APortal_LoanApprovalList":
                        List<GetLoanApplicationList> result2 = new List<GetLoanApplicationList>();
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string jsonResponse = JsonConvert.SerializeObject(response.Response);
                            result2 = JsonConvert.DeserializeObject<List<GetLoanApplicationList>>(jsonResponse);
                        }
                        return PartialView("_LoanApprovalList", result2);
                        break;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "List");
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
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "Create");
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
                        return PartialView("_AddAgents", result1.ToList().FirstOrDefault());
                        break;
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "Edit");
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
                await SaveErrorLog(ex, "FinanceController", "Delete");
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
                await SaveErrorLog(ex, "FinanceController", "Status");
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
                }
            }
            catch (Exception ex)
            {
                await SaveErrorLog(ex, "FinanceController", "ExportToExcel");
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
                await SaveErrorLog(ex1, "FinanceController", "SaveErrorLog");
            }
        }
        #endregion
    }
}