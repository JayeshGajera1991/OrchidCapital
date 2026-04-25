using System.ComponentModel.DataAnnotations;

namespace OrchidCapital.Models
{
    public class Login
    {

    }
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Minimum 6 characters")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string IsRepeat { get; set; }
        public string TwoFactorEnabled { get; set; }
    }
}