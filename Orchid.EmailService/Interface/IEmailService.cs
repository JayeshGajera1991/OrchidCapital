using Orchid.DataModels;

namespace Orchid.EmailService.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequestModel model);
    }
}
