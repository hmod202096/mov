using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class Paginginfo
    {
        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; } //عدد السجلات التي تريد أن تظهر بالصفحه
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalRecords / RecordsPerPage);
        public string UrlParam { get; set; }
    }
}
