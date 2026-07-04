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
        public string ErrorMessage { get; set; }
        public string PublicKey { get; set; }
    }
    public class LoginResponse
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
    public class ChangePasswordViewModel
    {
        public string PublicKey { get; set; }
        public string ErrorMessage { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }        
    }
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string PublicKey { get; set; }
        public string ErrorMessage { get; set; }
        [Required]
        public string ResetCode { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
    public class EmailConfiguration
    {
        public bool SSL { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SmtpServer { get; set; }
        public string Username { get; set; }
        public string ReplyToEmail { get; set; }
        public string FileAttachmentName { get; set; }
        public string FileAttachment { get; set; }
        public string OrgURL { get; set; }
    }

    public class TwoFactorViewModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string SecretKey { get; set; }
        public string AuthCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class TwoFactorRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string SecretKey { get; set; }
        public string AuthCode {get; set;}
        public bool IsTwoFactor { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SendWelcomeMailRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string SelUserName { get; set; }
    }
}