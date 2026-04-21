using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ProcurmentProject.Interfaces;

namespace ProcurmentProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendMail(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_config["EmailSetting:Email"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = message};
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(
                    _config["EmailSetting:Host"],
                    int.Parse(_config["EmailSetting:Port"]),
                    SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _config["EmailSetting:Email"],
                    _config["EmailSetting:Password"]
                );

                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
