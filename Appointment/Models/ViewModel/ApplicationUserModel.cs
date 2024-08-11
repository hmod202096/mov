using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter your Frist Name")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your User Name")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Display(Name = "الايميل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone")]
        [Display(Name = "رقم الجوال")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your Type User")]
        public int UserTypeId { get; set; }
        public List<UserTypes> UserTypesList { get; set; }

        public IList<string> Roles { get; set; }

        [Display(Name = "نشط")]
        public bool Active { get; set; }

        public ApplicationUserModel()
        {
            Roles = new List<string>();
        }

        [Required(ErrorMessage = "يجب اختيار اسم الفرع")]
        [Display(Name = "اسم الفرع")]
        public int BranchId { get; set; }
        public List<Branches> BranchesList { get; set; }            //تم استخدامها لنقل الموظف من فرع غلى آخر

       

    }
}
