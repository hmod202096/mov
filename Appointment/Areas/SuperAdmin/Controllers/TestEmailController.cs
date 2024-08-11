using Appointment.Models.ViewModel;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Appointment.Areas.SuperAdmin.Controllers
{

    [Area("SuperAdmin")]
    [Authorize(Roles = (SD.SuperAdminUser))]
    public class TestEmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly SMTPConfigViewModel _option;

        public TestEmailController(IEmailSender emailSender, IOptions<SMTPConfigViewModel> option)
        {
            _emailSender = emailSender;
            _option = option.Value;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Edit(string email, string subject, string htmlMessage)
        {

            try
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
            catch (Exception ex)
            {

                ex.Message.ToString();
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
