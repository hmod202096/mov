using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class NeighborhoodViewModel
    {
        public List<Neighborhoods> Neighborhoods { get; set; }
        public Paginginfo Paginginfo { get; set; }
    }
}
