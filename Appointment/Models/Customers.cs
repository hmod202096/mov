using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يجب ادخال اسم المتبرع")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        [Display(Name = "الجوال")]
        public string Mobily { get; set; }

        [Required(ErrorMessage = "يجب اختيار اسم الحي")]
        public int NeighId { get; set; }
        [ForeignKey("NeighId")]
        public Neighborhoods Neighborhoods { get; set; }

        public virtual ICollection<Reservations> Reservations { get; set; }


    }
}
