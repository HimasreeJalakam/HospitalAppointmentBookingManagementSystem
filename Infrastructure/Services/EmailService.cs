using Microsoft.Extensions.Options;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService

    {
        private readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }
        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(_settings.SmtpHost)
            {
                Port = _settings.SmtpPort,
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.FromEmail, _settings.AppPassword),
            };
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            var message = new MailMessage(_settings.FromEmail, toEmail, subject, body);
            //CreateSmtpClient().Send(message);
        }

    }


}
