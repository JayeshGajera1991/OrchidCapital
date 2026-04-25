
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
    public class AuthUserLogin
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string UserRole { get; set; }
        public string IsRepeat { get; set; }
    }
    public class Token
    {
        public string token { get; set; }
    }
    public class UserLoginResponse
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string UserRole { get; set; }
        public int LoginTryCount { get; set; }
        public int IsRepeat { get; set; }
    }
}
