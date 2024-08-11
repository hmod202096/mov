using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class UnfulfilledAppointmentViewModel
    {
        public Customers customers { get; set; }
        public List<Reservations> reservationsList { get; set; }
    }
}
