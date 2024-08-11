using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Branches_HistoryDates
    {
        public int BranchId { get; set; }
        public virtual Branches Branch { get; set; }

        public int HistoryDateId { get; set; }
        public virtual HistoryDate HistoryDate { get; set; }
        
        [Required]
        public int CountBooking { get; set; }
    }
}
