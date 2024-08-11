using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class CarAndCarTypeViewModel
    {
        public IEnumerable<CareTypes> CareTypesList { get; set; }
        public Cares Cares { get; set; }
        public string StatusMessage { get; set; }

    }
}
