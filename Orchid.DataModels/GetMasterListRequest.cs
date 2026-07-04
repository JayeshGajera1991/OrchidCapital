namespace Orchid.DataModels
{
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
        public int MenuId { get; set; }
        public bool IsActive { get; set; }
        public string PageName { get; set; }
        public string Icon { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public int OrderNo { get; set; }
    }
    public class UpdateBranchDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int BranchId { get; set; }

        public string? BranchCode { get; set; }

        public string BranchName { get; set; }

        public string BranchAddress { get; set; }

        public string BankName { get; set; }

        public string IFSCCode { get; set; }

        public string MICRCode { get; set; }

        public string ContactPerson { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
    public class UpdateImageConfigDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; }
    }
    public class UpdateTeamDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Qualification { get; set; }
        public string Headline { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
    }
    public class UpdateRoleDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
    }
    public class UpdateUsersDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public string CreateUserName { get; set; }
        public string? EncPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; }
        public string? ImageUrl { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string? EmergencyMobileNumber { get; set; }
        public string? CultureCode { get; set; }
        public string? UserDateFormat { get; set; }
        public bool? IsWCSend { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsRepeat { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsTwoFactor { get; set; }
        public string? SecretKey { get; set; }
        public int? ResetPESCount { get; set; }
        public string? ResetPESDate { get; set; }
        public int? ForgotPESCount { get; set; }
        public string? ForgotPESDate { get; set; }
        public int? WelcomeESCount { get; set; }
        public string? WelcomeESDate { get; set; }
        public string? ReferenceCode { get; set; }
        public int? PasswordTryCount { get; set; }
    }
    public class UpdateUserPageRightsMappingRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int UserId { get; set; }
        public string XmlData { get; set; }
    }
    public class ErrorLogModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string ExceptionMessage { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
    public class TwoFactorRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string SecretKey { get; set; }
        public bool IsTwoFactor { get; set; }
    }

    public class SendWelcomeMailRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string SelUserName { get; set; }
    }
    public class UpdateProfileImageRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string ImageUrl { get; set; }
    }
    public class DeleteProfileImageRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
    public class UpdateBankServiceDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public long LoanProductId { get; set; }

        public string? ProductCode { get; set; }

        public string ProductName { get; set; }

        public string LoanType { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }

        public int MinTenure { get; set; }

        public int MaxTenure { get; set; }

        public string InterestType { get; set; }

        public decimal DefaultInterestRate { get; set; }

        public decimal? ProcessingFeePercent { get; set; }

        public decimal? PenaltyInterest { get; set; }

        public int? GraceDays { get; set; }

        public bool? IsActive { get; set; }
    }
    
}