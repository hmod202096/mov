using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class ReservtionViewModel
    {
        public List<Reservations> ReservationsList { get; set; }
        public Paginginfo Paginginfo { get; set; }

        [Required(ErrorMessage ="حقل اجباري")]
        [StringLength(10, ErrorMessage = "خطأ في الادخال", MinimumLength = 10)]
        public string Mobily { get; set; }

    }
}
