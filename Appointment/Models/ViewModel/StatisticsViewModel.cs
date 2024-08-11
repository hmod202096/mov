using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class StatisticsViewModel
    {

        //[Display(Name = "من تاريخ")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime FromDate { get; set; }
        
        //[Display(Name = "إلى تاريخ")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime ToDate { get; set; }

        public int BookingDone { get; set; }
        public int DriverDone { get; set; }
        public int CompletedDone { get; set; }
        public int CancelDone { get; set; }
        public int DeleteDone { get; set; }
        public int Total { get; set; }
        public string StatusName { get; set; }

       // public IEnumerable<Reservations> ReservationsList { get; set; }
    }
}
