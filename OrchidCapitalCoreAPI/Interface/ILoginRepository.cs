using Orchid.DataModels;

namespace OrchidCapitalCoreAPI.Interface
{
    public interface ILoginRepository
    {
        Task<CommonResponse> AuthenticateUser(LoginRequest request);
        Task<CommonResponse> ChangePassword(ChangePassword request);
        Task<CommonResponse> ForgotPassword(ForgotPasswordViewModel request);
        Task<CommonResponse> GetUserPassword(string userName);
        Task<CommonResponse> ResetPassword(ResetPasswordViewModel request);
        Task<CommonResponse> VerifyResetPasswordRequest(string resetCode);
    }
}
