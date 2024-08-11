using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Neighborhoods
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="حقل اجباري")]
        [Display(Name ="اسم الحي")]
        public string Name { get; set; }

        public virtual ICollection<Branches_Neighborhoodes> Branches_Neighborhoodes { get; set; }

    }
}
