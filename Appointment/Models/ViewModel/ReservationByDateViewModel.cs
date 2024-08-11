using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class ReservationByDateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobily { get; set; }
        public string Neighborhood { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Donation { get; set; }
        public string Comments { get; set; }
        public string DriverName { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string UserName { get; set; }

    }
}
