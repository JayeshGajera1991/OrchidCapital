using Orchid.DataModels;
namespace OrchidCapitalCoreAPI.Interface
{
    public interface IAdminRepository
    {
        Task<CommonResponse> ChangeStatusInMasterDetails(ChangeStatusInMasterDetailsRequest request);
        Task<CommonResponse> DeleteMasterDetails(DeleteMasterDetailsRequest request);
        Task<CommonResponse> EditMasterDetails(int id,string SPName);
        Task<CommonResponse> GetMasterList(GetMasterListRequest request);
        Task<CommonResponse> UpdateConfigSettingsDetails(UpdateConfigSettingsDetailsRequest request);
        Task<CommonResponse> UpdateImageConfigDetails(UpdateImageConfigDetailsRequest request);
        Task<CommonResponse> UpdatePageDetails(UpdatePageDetailsRequest request);
        Task<CommonResponse> UpdateRoleDetails(UpdateRoleDetailsRequest request);
        Task<CommonResponse> UpdateTeamDetails(UpdateTeamDetailsRequest request);
        Task<CommonResponse> UpdateUserPageRightsMapping(UpdateUserPageRightsMappingRequest request);

    }
}