using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class Branch_HistoryDateViewModel
    {
        public int HistoryDateId { get; set; }
        public int BranchId { get; set; }

        [Display(Name = "التاريخ الميلادي")]
        public string GregorianDate { get; set; }

        [Display(Name = "التاريخ الهجري")]
        public string IslamicHistory { get; set; }

        [Display(Name = "اليوم")]
        public string Day { get; set; }

        [Required(ErrorMessage = "حقل اجباري")]
        [Display(Name = "الحد الأقصى لإظهار التاريخ")]
        [Range(0, 60, ErrorMessage = "الرجاء إدخال قيمة أكبر من أو تساوي 1 وأصغر من أو تساوي 60")]
        public int CountBooking { get; set; }

        
    }
}
