
using System.ComponentModel.DataAnnotations;

namespace OrchidCapital.Models
{
    public class FinanceModel
    {

    }
    public class CommonListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GetLoanApplicationList
    {
        public int LoanApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public long CustomerId { get; set; }
        public long LoanProductId { get; set; }
        public long BranchId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string BranchName { get; set; }
        public long ApplicationStatus { get; set; }
        public string Status { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal RequestedTenure { get; set; }
        public decimal RequestedInterestRate { get; set; }
        public string LoanPurpose { get; set; }
        public string Remarks { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public string? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public int TotalRecords { get; set; }
        public int LoanStep { get; set; }
    }
    public class GetLoanApplicationDocumentList
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int DocumentId { get; set; }
        public int? LoanApplicationId { get; set; }
        public int? LoanId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentName { get; set; }
        public string? FilePath { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime? UploadedDate { get; set; }
        public bool? Verified { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
    public class LoanApplicationDocumentViewModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int DocumentId { get; set; }
        public int LoanApplicationId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string? FilePath { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }
    }
    public class UpdateLoanApplicationDocumentRequest
    {
        public int LoanApplicationId { get; set; }

        public List<LoanApplicationDocumentViewModel> Documents { get; set; }
            = new();
    }
    public class UpdareLoanApplicationDocumentDetails
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int DocumentId { get; set; }
        public int LoanApplicationId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string? FilePath { get; set; }
    }

    public class UpdateLoanApplicationGuarantorRequest
    {
        public int LoanApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string CustomerName { get; set; }
        public decimal RequestedAmount { get; set; }
        public string BranchName { get; set; }

        public List<GuarantorViewModel> Guarantors { get; set; } = new();
    }

    public class GuarantorViewModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int GuarantorId { get; set; }
        public int LoanApplicationId { get; set; }
        public int? LoanId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "MobileNumber is required")]
        public string? Mobile { get; set; }
        [Required(ErrorMessage = "Occupation is required")]
        public string? Occupation { get; set; }
        [Required(ErrorMessage = "Relationship is required")]
        public string? Relationship { get; set; }
        [Required(ErrorMessage = "PANNumber is required")]
        public string? PANNumber { get; set; }
        [Required(ErrorMessage = "AadhaarNumber is required")]
        public string? AadhaarNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        public List<GuarantorDocumentViewModel> Documents { get; set; } = new();
    }

    public class UpdateGuarantorRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int GuarantorId { get; set; }
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
        public int GuarantorId { get; set; }
        public int GuarantorDocumentId { get; set; }
        [Required(ErrorMessage = "DocumentName is required")]
        public string DocumentName { get; set; }
        public string? FilePath { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile? ImageFile { get; set; }
    }

    public class LoadCommonResponseModel
    {
        public string Message { get; set; }
        public int? LoanApplicationId { get; set; }
        public int Id { get; set; }
    }
    public class UpdateFinalLoanApplicationDetails
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int LoanApplicationId { get; set; }
    }
    public class LoanApplicationReview()
    {
        public GetLoanApplicationList getLoanDetails { get; set; }
        public List<GetLoanApplicationDocumentList> documentLists { get; set; }
        public List<UpdateGuarantorRequest> guarantorRequests { get; set; }
    }
}