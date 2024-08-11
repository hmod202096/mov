using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Branches_Neighborhoodes
    {
        public Branches_Neighborhoodes()
        {
            Active = true;
        }

        public int BranchId { get; set; }
        public virtual Branches Branches { get; set; }

        public int NeighborhoodId { get; set; }
        public virtual Neighborhoods Neighborhoods { get; set; }

        public bool Active { get; set; }
    }
}
