using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public StatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StatisticsViewModel>> ReservationByDate(string fromDate, string toDate)
        {

            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var reservation = await _context.Reservations.Include(m => m.Branches)
                        .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date).ToListAsync();


            var grouped = from j in reservation
                          group j by new { j.Branches.Name }
                          into gr
                          select new StatisticsViewModel
                          {
                              StatusName = gr.Key.Name,
                              BookingDone = gr.Count(c => c.Status == (SD.BookingDone)),
                              DriverDone = gr.Count(c => c.Status == (SD.DriverDone)),
                              CompletedDone = gr.Count(c => c.Status == (SD.CompletedDone)),
                              CancelDone = gr.Count(c => c.Status == (SD.CancelDone)),
                              DeleteDone = gr.Count(c => c.Status == (SD.DeleteDone)),
                              Total = gr.Count()
                          };

            return grouped.ToList();
        }

        public async Task<List<Customers>> GetAllCustomer()
        {
            var data = await _context.Customers.Include(m => m.Neighborhoods).ToListAsync();

            return data;
        }

        //عدد المواعيد حسب اسم المستخدم
        public async Task<List<CountAppByUserViewModel>> CountReservationByUserRepo(string fromDate = null, string toDate = null)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var branch = await _context.Branches.ToListAsync();

            var reservation = await _context.Reservations
                                .Include(x => x.Branches)
                                .Include(x => x.ApplicationUser)
                                .Where(x => x.AppointmentDate >= d1.Date && x.AppointmentDate <= d2.Date)
                                .OrderBy(m => m.Branches.Name).ToListAsync();

            var grouped = from j in reservation
                          group j by new { j.ApplicationUser.Name, j.ApplicationUser.Branch }
                          into gr
                          select new CountAppByUserViewModel
                          {
                              BranchName = gr.Key.Branch.Name,
                              EmpName = gr.Key.Name,
                              CountApp = gr.Count(),
                              Branches = branch
                          };

            return grouped.OrderByDescending(x => x.CountApp).ToList();



        }

    }
}
