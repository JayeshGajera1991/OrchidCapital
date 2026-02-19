using Orchid.DataModels;

namespace OrchidCapitalCoreAPI.Interface
{
    public interface ILoginRepository
    {
        Task<CommonResponse> AuthenticateUser(LoginRequest request);
    }
}
