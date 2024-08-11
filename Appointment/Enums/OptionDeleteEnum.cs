using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Enums
{
    public enum OptionDeleteEnum
    {
        [Display(Name = "لم يتم الرد ********  No response")]
        NoResponse = 1,
        [Display(Name = "تم تأجيل الموعد ******** Delay")]
        Delay = 2,
        [Display(Name = "الأثاث غير صالح للاستخدام ******** Expired")]
        NotGood = 3
    }
}
