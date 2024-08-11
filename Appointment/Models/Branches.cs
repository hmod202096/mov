using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Branches
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "حقل اجباري")]
        [Display(Name = "اسم الفرع")]
        public string Name { get; set; }

        [Required(ErrorMessage = "حقل اجباري")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Display(Name = "التنويه")]
        public string Message { get; set; }

        [Required(ErrorMessage = "حقل اجباري")]
        [Display(Name = "الحد الأقصى لإظهار التاريخ")]
        [Range(1, 30, ErrorMessage = "الرجاء إدخال قيمة أكبر من أو تساوي 1 وأصغر من أو تساوي 30")]
        public int MaxDate { get; set; }

        public virtual ICollection<Branches_HistoryDates> Branches_HistoryDates { get; set; }
        public virtual ICollection<Branches_Neighborhoodes> Branches_Neighborhoodes { get; set; }
    }
}
