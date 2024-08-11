using Appointment.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class OptionDeleteAppointmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="يجب اختيار السبب")]
        public string Notes { get; set; }

        public string Dat { get; set; }

    }
}
