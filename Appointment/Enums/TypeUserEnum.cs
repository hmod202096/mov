using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Enums
{
    public enum TypeUserEnum
    {
        [Display(Name = "مدير الفرع")]
        Manager = 1,
        [Display(Name = "مسؤول الحركة")]
        Supervisor = 2,
        [Display(Name = "موظف")]
        Emplooye = 3,
        [Display(Name = "سائق")]
        Driver = 4,
        [Display(Name = "عام")]
        Customer = 5
    }
}
