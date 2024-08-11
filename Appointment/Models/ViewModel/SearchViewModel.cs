using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class SearchViewModel
    {
        public string Name { get; set; }

        public string Mobily { get; set; }

        public string Neighporhood { get; set; }

        public IEnumerable<Reservations> Reservations { get; set; }

    }
}
