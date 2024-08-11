using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class ReservationOperationViewModel
    {
        public ReservationOperationViewModel()
        {
            CountAppointment = 1;
        }

        public Customers Customers { get; set; }
        public Reservations Reservations { get; set; }
        public List<Neighborhoods> Neighborhoods { get; set; }

        [Required(ErrorMessage ="حقل اجباري")]
        [Display(Name ="عدد المواعيد")]
        [Range(1, 5,
        ErrorMessage = "يجب أن تكون القيمة ما بين 1 و 5")]
        public int CountAppointment { get; set; }

        public int MaxDate { get; set; }

        public bool PageDetailesNotDoneAppointment { get; set; }
    }
}
