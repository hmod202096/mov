using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class CustomerViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يجب ادخال اسم المتبرع")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        [Display(Name = "الجوال")]
        [StringLength(10, ErrorMessage = "خطأ في الادخال", MinimumLength = 10)]
        public string Mobily { get; set; }

        public string NeighborhoodsName { get; set; }

        [Required(ErrorMessage = "يجب اختيار اسم الحي")]
        [Display(Name = "اسم الحي")]
        public int NeighId { get; set; }
        public List<Neighborhoods> NeighborhoodsList { get; set; }

        public string StatusMessage { get; set; }
    }
}
