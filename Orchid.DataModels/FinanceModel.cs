namespace Orchid.DataModels
{
    public class FinanceModel
    {
    }
    public class UpdateLoanApplicationDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public long? LoanApplicationId { get; set; }
        public string? ApplicationNo { get; set; }
        public long CustomerId { get; set; }
        public long LoanProductId { get; set; }
        public long? BranchId { get; set; }
        public decimal RequestedAmount { get; set; }
        public int RequestedTenure { get; set; }
        public decimal? RequestedInterestRate { get; set; }
        public string LoanPurpose { get; set; }
        public int? ApplicationStatus { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long? ApprovedBy { get; set; }
        public string Remarks { get; set; }
    }
    public class UpdareLoanApplicationDocumentDetails
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int? DocumentId { get; set; }
        public int LoanApplicationId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string? FilePath { get; set; }
    }

    public class UpdateGuarantorRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int? GuarantorId { get; set; }
        public int LoanApplicationId { get; set; }
        public int? LoanId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Occupation { get; set; }
        public string Relationship { get; set; }
        public string PANNumber { get; set; }
        public string AadhaarNumber { get; set; }
        public string Address { get; set; }
    }
    public class GuarantorDocumentViewModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int? GuarantorId { get; set; }
        public int? GuarantorDocumentId { get; set; }
        public string DocumentName { get; set; }
        public string? FilePath { get; set; }
    }
    public class UpdateFinalLoanApplicationDetails
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int LoanApplicationId { get; set; }
    }
}