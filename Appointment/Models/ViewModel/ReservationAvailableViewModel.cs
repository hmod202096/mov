using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class ReservationAvailableViewModel
    {
        public DateTime Gregorian { get; set; }
        public string Hijri { get; set; }
        public string DayName { get; set; }
        public int CountBooking { get; set; }       //المسموح بها
        public int Booked { get; set; }             //المحجوز
        public int Available { get; set; }          //المتاح

        [Required(ErrorMessage = "حقل اجباري")]
        [StringLength(10, ErrorMessage = "خطأ في الادخال", MinimumLength = 10)]
        public string Mobily { get; set; }          //تمرير رقم الجوال الى صفحة الحجوزات
        public string DateTime { get; set; }        //تمرير التاريخ الى صفحة الحجوزات

    }
}
