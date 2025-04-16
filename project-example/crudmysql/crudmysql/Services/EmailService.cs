using MailKit;
using MimeKit;
using crudmysql.Services.Interfaces;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace crudmysql.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var from = _config["Email:From"];
            if (string.IsNullOrWhiteSpace(from)) throw new ArgumentNullException(nameof(from), "Email:From is not configured.");
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentNullException(nameof(to), "Recipient email is required.");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("sandbox.smtp.mailtrap.io", 2525, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

    }
}
