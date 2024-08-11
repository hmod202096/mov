using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class HistoryDate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual ICollection<Branches_HistoryDates> Branches_HistoryDates { get; set; }
    }
}
