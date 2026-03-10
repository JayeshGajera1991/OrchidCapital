using Orchid.EmailService.Interface;
using Orchid.DataModels;
using MimeKit;
using Microsoft.Extensions.Options;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Orchid.EmailService.Repository
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _config;

        public EmailService(IOptions<EmailConfiguration> config)
        {
            _config = config.Value;
        }
        public async Task<bool> SendEmailAsync(EmailRequestModel model)
        {
            bool isSent = false;
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(model.EmailFrom)
            };

            email.To.Add(MailboxAddress.Parse(model.EmailToId));
            //email.From.Add(MailboxAddress.Parse(model.EmailFrom));

            if (Convert.ToString(model.EmailBCC).Trim() != "")
                email.Bcc.Add(MailboxAddress.Parse(model.EmailBCC));

            if (Convert.ToString(model.EmailCC).Trim() != "")
                email.Cc.Add(MailboxAddress.Parse(model.EmailCC));

            email.Subject = model.EmailSubject;
            var builder = new BodyBuilder();
            builder.HtmlBody = model.EmailBody;
            if (!string.IsNullOrEmpty(model.FilePath))
            {
                builder.Attachments.Add(model.FilePath);
            }
            email.Body = builder.ToMessageBody();
            try
            {
               
                var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config.SmtpServer, _config.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config.Username, _config.Password);

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
               
                isSent = true;
            }
            catch (Exception ex)
            {
                isSent = false;
            }
            return isSent;
        }
    }
}
