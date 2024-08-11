using Appointment.Models.ViewModel;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Appointment.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly SMTPConfigViewModel _option;

        public EmailSender(IOptions<SMTPConfigViewModel> option)
        {
            _option = option.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var fromMail = _option.UserName;
            var fromPassword = _option.Password;

            var message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(email);
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;

            var stmpClient = new SmtpClient(host: _option.Host)
            {
                Port = _option.Port,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = _option.EnableSSL
            };

           stmpClient.Send(message);
        }
    }
}
