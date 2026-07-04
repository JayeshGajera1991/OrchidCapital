using Orchid.DataModels;
namespace OrchidCapitalCoreAPI.Interface
{
    public interface IAdminRepository
    {
        Task<CommonResponse> ChangeStatusInMasterDetails(ChangeStatusInMasterDetailsRequest request);
        Task<CommonResponse> DeleteMasterDetails(DeleteMasterDetailsRequest request);
        Task<CommonResponse> DeleteProfileImage(DeleteProfileImageRequest request);
        Task<CommonResponse> DeleteUser(SendWelcomeMailRequest request);
        Task<CommonResponse> EditMasterDetails(string userName, int id, string SPName);
        Task<CommonResponse> ErrorLog(ErrorLogModel request);
        Task<CommonResponse> GetMasterList(string UserName, string UserRole, int IsActive, int PageNumber, int PageSize, string SortColumn, string SortDirection, string SPName, string Filter = "");
        Task<CommonResponse> GetMenuList();
        Task<CommonResponse> GetRolePermissionsList(int id);
        Task<CommonResponse> GetUserLoginList();
        Task<CommonResponse> GetUserPageRightsList(string userName);
        Task<CommonResponse> ResetPasswordUser(SendWelcomeMailRequest request);
        Task<CommonResponse> ResetTwoFactorAuthentication(SendWelcomeMailRequest request);
        Task<CommonResponse> SendWelcomeMail(SendWelcomeMailRequest request);
        Task<CommonResponse> TwoFactorAuthentication(TwoFactorRequest request);
        Task<CommonResponse> UnlockUser(SendWelcomeMailRequest request);
        Task<CommonResponse> UpdateBankServiceDetails(UpdateBankServiceDetailsRequest request);
        Task<CommonResponse> UpdateBranchDetails(UpdateBranchDetailsRequest request);
        Task<CommonResponse> UpdateCustomersDetails(UpdateUsersDetailsRequest request);
        Task<CommonResponse> UpdateImageConfigDetails(UpdateImageConfigDetailsRequest request);
        Task<CommonResponse> UpdatePageDetails(UpdatePageDetailsRequest request);
        Task<CommonResponse> UpdateProfileImage(UpdateProfileImageRequest request);
        Task<CommonResponse> UpdateRoleDetails(UpdateRoleDetailsRequest request);
        Task<CommonResponse> UpdateTeamDetails(UpdateTeamDetailsRequest request);
        Task<CommonResponse> UpdateUserPageRightsMapping(UpdateUserPageRightsMappingRequest request);
        Task<CommonResponse> UpdateUsersDetails(UpdateUsersDetailsRequest request);
    }
}