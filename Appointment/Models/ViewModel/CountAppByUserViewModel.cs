using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class CountAppByUserViewModel
    {
        public string BranchName { get; set; }
        public string EmpName { get; set; }    
        public int CountApp { get; set; } = 0;

        public List<Branches> Branches { get; set; }
    }
}
