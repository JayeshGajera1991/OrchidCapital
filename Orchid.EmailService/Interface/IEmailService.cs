using Orchid.DataModels;

namespace Orchid.EmailService.Interface
{
    public interface IEmailService
    {
        Task<Tuple<bool,string>> SendEmailAsync(EmailRequestModel model);
    }
}
