using Appointment.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class DistributeViewModel
    {
        public Reservations Reservations { get; set; }
        public List<ApplicationUser> applicationUsersList { get; set; }
        public int MaxDate { get; set; }


    }
}
