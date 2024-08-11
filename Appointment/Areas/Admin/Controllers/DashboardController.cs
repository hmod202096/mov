using Appointment.Data;
using Appointment.Models.ViewModel;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (SD.AdminUser))]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Index()
        {
            return View();
        }

        //json
        // Get Count Appointment by Barnch Name()
        //url:/Admin/Dashboard/GetCountAppointment
        [HttpPost]
        [AllowAnonymous]
        public async Task<List<object>> GetCountAppointment()
        {
            var model = await _context.Reservations.Include(m => m.Branches)
                .GroupBy(b => new { b.Branches.Name })
                .Select(b => new DashboardViewModel()
                {
                    BranchName = b.Key.Name,
                    Total = b.Count()
                })
                .OrderBy(m => m.Total).ToListAsync();

            List<object> data = new List<object>();
            List<string> lables = new List<string>();
            List<int> numberBooking = new List<int>();

            foreach (var item in model)
            {
                lables.Add(item.BranchName);
                numberBooking.Add(item.Total);
            }

            data.Add(lables);
            data.Add(numberBooking);

            return data;
        }

    }
}
