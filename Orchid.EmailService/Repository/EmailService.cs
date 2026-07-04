using Orchid.EmailService.Interface;
using Orchid.DataModels;
using MimeKit;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using Orchid.UtilityHelper;

namespace Orchid.EmailService.Repository
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _config;

        public EmailService(IOptions<EmailConfiguration> config)
        {
            _config = config.Value;
        }
        public async Task<Tuple<bool, string>> SendEmailAsync(EmailRequestModel model)
        {
            bool isSent = false;
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(model.EmailFrom))
            {
                errorMessage = "EmailFrom is required fields.";
                return new Tuple<bool, string>(false, errorMessage);
            }

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(model.EmailFrom)
            };

            email.To.Add(MailboxAddress.Parse(model.EmailToId));

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
                await smtp.AuthenticateAsync(_config.Username, Encryption.Decrypt(Environment.GetEnvironmentVariable("USR_ENC_KEY"),_config.Password));

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                isSent = true;
                errorMessage = "Email sent successfully.";
            }
            catch (Exception ex)
            {
                isSent = false;
                return new Tuple<bool, string>(isSent, ex.Message);
            }
            return new Tuple<bool, string>(isSent, errorMessage);
        }
    }
}
