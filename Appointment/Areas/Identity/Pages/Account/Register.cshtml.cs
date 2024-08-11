using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Repositories;
using Appointment.Service;
using Appointment.TagHelpers;
using Appointment.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Appointment.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
             RoleManager<IdentityRole> roleManager,
             ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [MyCustomValidationAttribute("Gmail.com")]
            [Required(ErrorMessage = "أدخل الايميل")]
            [EmailAddress]
            [Display(Name = "الايميل")]
            public string Email { get; set; }

            //[Required]
            //[StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "كلمة المرور")]
            //public string Password { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "تأكيد كلمة المرور")]
            //[Compare("Password", ErrorMessage = "كلمة المرور وكلمة المرور التأكيدية غير متطابقتين.")]
            //public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "يجب ادخال الاسم")]
            [StringLength(20, ErrorMessage = "يجب أن لا يتجاوز 20 حرف وان لا يقل عن 5 أحرف", MinimumLength = 5)]
            [Display(Name = "الاسـم")]
            public string Name { get; set; }

            [Display(Name = "المدينه")]
            public string City { get; set; }

            [Required(ErrorMessage = "اختر نوع المستخدم")]
            [Display(Name = "نوع المستخدم")]
            public int UserTypeId { get; set; }

            [Required(ErrorMessage = "يجب ادخال رقم الجوال")]
            [Display(Name = "رقم الجوال")]
            [StringLength(10, ErrorMessage = "يجب أن يكون عشرة أرقام", MinimumLength = 10)]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "اختر اسم الفرع")]
            [Display(Name = "اسم الفرع")]
            public int BranchId { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    //UserName = new MailAddress(Input.Email).User,
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    City = Input.City,
                    UserTypesId = Input.UserTypeId,
                    PhoneNumber = Input.PhoneNumber,
                    BranchId = Input.BranchId,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "Ha@123456");

                //var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                //var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    
                    //Send Email
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //تم استخدام كود نسيت الرقم السري بدلا من تأكيد الإيميل
                    //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ResetPassword",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(
                    //    Input.Email,
                    //    "Reset Password",
                    //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    // Add Role
                    await _userManager.AddToRoleAsync(user, SD.EmployeekUser);
                    //return RedirectToAction("Index", "Users", new { Area = "Admin" });
                    
                    return RedirectToPage("./MessageSendEmail");

                }
              
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
