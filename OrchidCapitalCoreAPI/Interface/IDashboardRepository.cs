using Orchid.DataModels;

namespace OrchidCapitalCoreAPI.Interface
{
    public interface IDashboardRepository
    {
        Task<CommonResponse> GetToolsCountsList(string userName, string userRole);
    }
}