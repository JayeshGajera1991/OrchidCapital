using System.ComponentModel.DataAnnotations;

namespace OrchidCapital.Models
{
    public class AdminModel
    {

    }
    public class GetUserDetailsModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EncPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string EmergencyMobileNumber { get; set; }
        public string CultureCode { get; set; }
        public string UserDateFormat { get; set; }
        public bool IsWCSend { get; set; }
        public bool IsLocked { get; set; }
        public bool IsRepeat { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsTwoFactor { get; set; }
        public string SecretKey { get; set; }
        public int ResetPESCount { get; set; }
        public string ResetPESDate { get; set; }
        public int ForgotPESCount { get; set; }
        public string ForgotPESDate { get; set; }
        public int WelcomeESCount { get; set; }
        public string WelcomeESDate { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string ReferenceCode { get; set; }
        public int PasswordTryCount { get; set; }
        public string RoleName { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetUserDetailsResponseModel
    {
        public GetUserDetailsModel SingleResult { get; set; }
        public List<GetUserPageRightsList> PageRights { get; set; }
    }
    public class BankBranchModel
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int TotalRecords { get; set; }
    }
    public class PageModel
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string PageName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedOn { get; set; }
        public int TotalRecords { get; set; }
        public string Icon { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public int OrderNo { get; set; }
    }
    public class RoleModel
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class LoanTypeModel
    {
        public int LoanTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class ImageConfigModel
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class TeamModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Qualification { get; set; }
        public string Headline { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class ErrorLogListModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ErrorMessage { get; set; }
        public string CreatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class AuditLogListModel
    {
        public int Id { get; set; }
        public string CreatedUser { get; set; }
        public string OperationType { get; set; }
        public string Entity { get; set; }
        public string PrimaryKey { get; set; }
        public string SPName { get; set; }
        public string OldDataXml { get; set; }
        public string NewDataXml { get; set; }
        public string CreatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class GetUserPageRightsList
    {
        public string Id { get; set; }
        public string PageName { get; set; }
        public string IsSelected { get; set; }
        public string Icon { get; set; }
    }
    public class GetMasterListRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int IsActive { get; set; }
        public string SPName { get; set; }
    }
    public class DeleteMasterDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public string SPName { get; set; }
    }
    public class ChangeStatusInMasterDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string SPName { get; set; }
    }
    public class UpdatePageDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Please select menu name")]
        public int MenuId { get; set; }
        [Required(ErrorMessage = "Page name is required")]
        [StringLength(1000, ErrorMessage = "Page name cannot exceed 1000 characters")]
        public string PageName { get; set; }
        [Required(ErrorMessage = "Page icon is required")]
        [StringLength(100, ErrorMessage = "Page icon cannot exceed 100 characters")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "Action name is required")]
        [StringLength(500, ErrorMessage = "Action name cannot exceed 500 characters")]
        public string ActionName { get; set; }
        [Required(ErrorMessage = "Controller name is required")]
        [StringLength(500, ErrorMessage = "Controller name cannot exceed 500 characters")]
        public string Controller { get; set; }
        [Required(ErrorMessage = "Order number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Order number must be a positive integer")]
        public int OrderNo { get; set; }
    }
    public class UpdateUserDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public string CreateUserName { get; set; }
        public string EncPassword { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid mobile number")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Please select role")]
        public int RoleId { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(1000, ErrorMessage = "Address cannot exceed 1000 characters")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please select country")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Please select state")]
        public string State { get; set; }
        [Required(ErrorMessage = "Please select city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Pin Code is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Pin Code")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "DOB is required")]
        [StringLength(10, ErrorMessage = "DOB cannot exceed 10 characters")]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "Invalid DOB format. Use DD/MM/YYYY.")]
        public string DOB { get; set; }
        [Required(ErrorMessage = "Emergency Mobile Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Emergency Mobile Number")]
        public string EmergencyMobileNumber { get; set; }
        public string CultureCode { get; set; }
        public string UserDateFormat { get; set; }
        public bool IsWCSend { get; set; }
        public bool IsLocked { get; set; }
        public bool IsRepeat { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsTwoFactor { get; set; }
        public string SecretKey { get; set; }
        public int ResetPESCount { get; set; }
        public string ResetPESDate { get; set; }
        public int ForgotPESCount { get; set; }
        public string ForgotPESDate { get; set; }
        public int WelcomeESCount { get; set; }
        public string WelcomeESDate { get; set; }
        public string ReferenceCode { get; set; }
        public int PasswordTryCount { get; set; }
    }
    public class UpdateBranchDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters")]
        public string BranchName { get; set; }
        [Required(ErrorMessage = "Branch address is required")]
        [StringLength(1000, ErrorMessage = "Branch address cannot exceed 1000 characters")]
        public string BranchAddress { get; set; }
        [Required(ErrorMessage = "Bank name is required")]
        [StringLength(500, ErrorMessage = "Bank name cannot exceed 500 characters")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "IFSC code is required")]
        [StringLength(11, ErrorMessage = "IFSC code must be exactly 11 characters")]
        public string IFSCCode { get; set; }
        [Required(ErrorMessage = "MICR code is required")]
        [StringLength(9, ErrorMessage = "MICR code must be exactly 9 characters")]
        public string MICRCode { get; set; }
        [Required(ErrorMessage = "Contact person is required")]
        [StringLength(100, ErrorMessage = "Contact person cannot exceed 100 characters")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?\d{0,10}$", ErrorMessage = "Invalid phone number")]
        [StringLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
    public class UpdateImageConfigDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Headline is required")]
        [StringLength(1000, ErrorMessage = "Headline cannot exceed 1000 characters")]
        public string Headline { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Role is required")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }
        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        public string Type { get; set; }
    }
    public class UpdateTeamDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Role is required")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(1000, ErrorMessage = "Qualification cannot exceed 1000 characters")]
        public string Qualification { get; set; }
        [Required(ErrorMessage = "Headline is required")]
        [StringLength(1000, ErrorMessage = "Headline cannot exceed 1000 characters")]
        public string Headline { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description1 { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description2 { get; set; }
    }
    public class UpdateRoleDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Role is required")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        public string Role { get; set; }
    }
    public class UpdateUserPageRightsMappingRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int UserId { get; set; }
        public string XmlData { get; set; }
    }
    public class AddRolePermissionsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int UserId { get; set; }
        public List<GetRolePermissionsRequest> RolePermissions { get; set; }
    }
    public class GetRolePermissionsList
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string PageName { get; set; }
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public bool IsAdd { get; set; }
        public bool IsView { get; set; }
        public string IsSelected { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public string IconName { get; set; }
    }
    public class GetRolePermissionsRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public int IsRights { get; set; }
    }
    public class ErrorLogModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string ExceptionMessage { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
    public class CommonResponseModel
    {
        public string Message { get; set; }
    }
    public class ResetPasswordUserModel
    {
        public string EncPassword { get; set; }
    }
    public class ImageConfigurationModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class UpdateProfileImageRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
    }
    public class DeleteProfileImageRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
    public class MenuModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateBankServiceDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public long LoanProductId { get; set; }
        public string ProductCode { get; set; }
        public bool IsSecure { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Descriptions { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(500, ErrorMessage = "Product name cannot exceed 500 characters")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Please select loan type")]
        public int LoanTypeId { get; set; }
        [Required(ErrorMessage = "Min amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Min amount must be a positive number")]
        public decimal? MinAmount { get; set; }
        [Required(ErrorMessage = "Max amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Max amount must be a positive number")]
        public decimal? MaxAmount { get; set; }
        [Required(ErrorMessage = "Min tenure is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Min tenure must be a positive integer")]
        public int? MinTenure { get; set; }
        [Required(ErrorMessage = "Max tenure is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Max tenure must be a positive integer")]
        public int? MaxTenure { get; set; }
        [Required(ErrorMessage = "Please select interest type")]
        public string InterestType { get; set; }
        [Required(ErrorMessage = "Default interest rate is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Default interest rate must be a positive number")]
        public decimal? DefaultInterestRate { get; set; }
        [Required(ErrorMessage = "Processing fee percent is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Processing fee percent must be a positive number")]
        public decimal? ProcessingFeePercent { get; set; }
        [Required(ErrorMessage = "Penalty interest is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Penalty interest must be a positive number")]
        public decimal? PenaltyInterest { get; set; }
        [Required(ErrorMessage = "Grace days is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Grace days must be a positive integer")]
        public int? GraceDays { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsActive { get; set; }
    }
    public class UpdateLoanTypeDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public long LoanTypeId { get; set; }
        [Required(ErrorMessage = "Loan type is required")]
        [StringLength(500, ErrorMessage = "Loan type cannot exceed 500 characters")]
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
    public class GetNewApplicationNumber
    {
        public string ApplicationNumber { get; set; }
    }
    public class UpdateLoanApplicationDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public long? LoanApplicationId { get; set; }
        public string? ApplicationNo { get; set; }
        [Required(ErrorMessage = "Please select customer")]
        public long CustomerId { get; set; }
        [Required(ErrorMessage = "Please select service")]
        public long LoanProductId { get; set; }
        [Required(ErrorMessage = "Please select branch")]
        public long? BranchId { get; set; }
        [Required(ErrorMessage = "Requested amount is required")]
        public decimal RequestedAmount { get; set; }
        [Required(ErrorMessage = "Requested tenure is required")]
        public int? RequestedTenure { get; set; }
        [Required(ErrorMessage = "Requested interest rate is required")]
        public decimal? RequestedInterestRate { get; set; }
        [Required(ErrorMessage = "Loan purpose is required")]
        public string LoanPurpose { get; set; }
        [Required(ErrorMessage = "Please select status")]
        public int ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long? ApprovedBy { get; set; }
        [Required(ErrorMessage = "Remarks is required")]
        public string Remarks { get; set; }
    }
    public class BankServiceListModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public long LoanProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LoanType { get; set; }
        public int LoanTypeId { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public int MinTenure { get; set; }
        public int MaxTenure { get; set; }
        public string InterestType { get; set; }
        public decimal DefaultInterestRate { get; set; }
        public decimal ProcessingFeePercent { get; set; }
        public decimal PenaltyInterest { get; set; }
        public int GraceDays { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSecure { get; set; }
        public string Descriptions { get; set; }
        public int TotalRecords { get; set; }
    }
}