using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class DashboardViewModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int BookingDone { get; set; }
        public int DriverDone { get; set; }
     
        public int CancelDone { get; set; }
        public int DeleteDone { get; set; }

        public int CompletedDone { get; set; }          // المواعيد المنفذه  Count(c => c.Status == (SD.CompletedDone)),
        public int Unfulfilled { get; set; }            //المواعيد المتعثرة Count(c => c.Status == SD.CancelDone),
        public int Unexecuted { get; set; }             //المواعيد الغير منفذه Count(c => c.Status == SD.BookingDone || c.Status == SD.DriverDone || c.Status == SD.DeleteDone),
        public int Total { get; set; }                  // إجمالي المواعيد

        public int TotalUser { get; set; }
        public int TotalDriver { get; set; }
        public string NeighName { get; set; }
        public int CountNeigh { get; set; }
        public string DriverName { get; set; }
        public int CountDriverExu { get; set; }
        public int CountAppo { get; set; }
        public int CountDonor { get; set; }

    }
}
