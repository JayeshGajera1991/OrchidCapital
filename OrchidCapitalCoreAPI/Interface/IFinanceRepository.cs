using Orchid.DataModels;

namespace OrchidCapitalCoreAPI.Interface
{
    public interface IFinanceRepository
    {
        Task<CommonResponse> GetFinanceList(string UserName, string UserRole, int IsActive, int PageNumber, int PageSize, string SortColumn, string SortDirection, string SPName, string Filter = "");
        Task<CommonResponse> EditFinanceDetails(string userName, int id, string SPName);
        Task<CommonResponse> DeleteFinanceDetails(DeleteMasterDetailsRequest request);
        Task<CommonResponse> ChangeStatusInFinanceDetails(ChangeStatusInMasterDetailsRequest request);
        Task<CommonResponse> UpdateLoanApplicationDetails(UpdateLoanApplicationDetailsRequest request);
        Task<CommonResponse> GetCommonList(string userName, string userRole, string option);
        Task<CommonResponse> UpdateLoanDocumentDetails(UpdareLoanApplicationDocumentDetails request);
        Task<CommonResponse> UpdateLoanGuarantorDetails(UpdateGuarantorRequest request);
        Task<CommonResponse> UpdateLoanGuarantorDocumentDetails(GuarantorDocumentViewModel request);
        Task<CommonResponse> UpdateFinalLoanApplicationDetails(UpdateFinalLoanApplicationDetails request);
        Task<CommonResponse> GetNewApplicationNumber(string userName, string userRole);

    }
}