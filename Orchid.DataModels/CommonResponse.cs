
namespace Orchid.DataModels
{
    public class CommonResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Response { get; set; }
    }
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PasswordTryCount { get; set; }
    }
    public class ChangePassword
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        public string EmailId { get; set; }
    }
    public class ResetPasswordViewModel
    {
        public string ResetCode { get; set; }
        public string NewPassword { get; set; }
    }
    public class AuthUserLogin
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string UserRole { get; set; }
        public string IsRepeat { get; set; }
        public string Message { get; set; }
        public string ResetCode { get; set; }
    }
    public class Token
    {
        public string token { get; set; }
    }
    public class UserLoginResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public bool IsRepeat { get; set; }
        public bool IsTwoFactor { get; set; }
        public string SecretKey { get; set; }
        public string CultureCode { get; set; }
        public string UserDateFormat { get; set; }
        public string ReferenceCode { get; set; }
        public string ImageUrl { get; set; }
        public int LoginTryCount { get; set; }
        public string Message { get; set; }
    }
}
