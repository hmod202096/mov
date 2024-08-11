using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Cares
    {
        public Cares()
        {
            Active = true;
        }

        [Key]
        [Display(Name ="رقم اللوحة")]
        public string PlatId { get; set; }

        [Display(Name = "موديل السيارة")]
        public string model { get; set; }
        
        [Display(Name = "نشط")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "نوع السيارة")]
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual CareTypes CareTypes { get; set; }

        [Required]
        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branches Branch { get; set; }
    }
}
