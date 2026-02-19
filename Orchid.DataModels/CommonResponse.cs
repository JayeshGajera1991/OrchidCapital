using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string UserRole { get; set; }
        public int PasswordTryCount { get; set; }
    }
    public class AuthUserLogin
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string UserRole { get; set; }
    }
    public class Token
    {
        public string token { get; set; }
    }
}
