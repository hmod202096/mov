using Appointment.Data;
using Appointment.Enums;
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
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //------------------------لوحة القيادة الأولى-------------------

        public async Task<List<DashboardViewModel>> CountUsersRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var countAppo = _context.Reservations.Count();
            var user = await _context.ApplicationUsers.Include(m => m.UserTypes).ToListAsync();

            var grouped = from j in user
                          group j by new { j.Id }
                         into gr
                          select new DashboardViewModel
                          {
                              TotalDriver = gr.Count(c => c.UserTypesId == ((int)TypeUserEnum.Driver)),
                              TotalUser = gr.Count(),
                              CountAppo = countAppo
                          };

            return grouped.ToList();
        }

        //جميع العمليات
        public async Task<List<Object>> AllOperationsRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var reservation = await _context.Reservations.Include(m => m.Branches)
                       .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date)
                       .ToListAsync();

            var model = from j in reservation
                        group j by new { j.Branches.Name }
                        into gr
                        select new DashboardViewModel
                        {
                            BranchName = gr.Key.Name,
                            Unexecuted = gr.Count(c => c.Status == SD.BookingDone || c.Status == SD.DriverDone || c.Status == SD.DeleteDone),
                            Unfulfilled = gr.Count(c => c.Status == SD.CancelDone),
                            CompletedDone = gr.Count(c => c.Status == (SD.CompletedDone)),
                            Total = gr.Count()
                        };

            List<object> data = new List<object>();
            List<string> branchName = new List<string>();
            List<int> unexecuted = new List<int>();
            List<int> unfulfilled = new List<int>();
            List<int> completedDone = new List<int>();
            List<int> total = new List<int>();

            foreach (var item in model)
            {
                branchName.Add(item.BranchName);
                unexecuted.Add(item.Unexecuted);
                unfulfilled.Add(item.Unfulfilled);
                completedDone.Add(item.CompletedDone);
                total.Add(item.Total);
            }
            data.Add(branchName);
            data.Add(unexecuted);
            data.Add(unfulfilled);
            data.Add(completedDone);
            data.Add(total);
            return data;
        }


        //عدد المواعيد
        public async Task<List<Object>> MaxBookingByBranchRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var model = await _context.Reservations.Include(m => m.Branches)
                       .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date)
                       .GroupBy(b => new { b.Branches.Name })
                                               .Select(b => new DashboardViewModel()
                                               {
                                                   BranchName = b.Key.Name,
                                                   CountAppo = b.Count()
                                               })
                                               .OrderByDescending(m => m.CountAppo)
                                               .ToListAsync();

            List<object> data = new List<object>();
            List<string> branchName = new List<string>();
            List<int> countAppo = new List<int>();

            foreach (var item in model)
            {
                branchName.Add(item.BranchName);
                countAppo.Add(item.CountAppo);
            }
            data.Add(branchName);
            data.Add(countAppo);
            return data;
        }

        public async Task<List<Object>> MaxCompletedByBranchRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var model = await _context.Reservations.Include(m => m.Branches)
                       .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date
                       && m.Status == SD.CompletedDone)
                       .GroupBy(b => new { b.Branches.Name })
                       .Select(b => new DashboardViewModel()
                       {
                           BranchName = b.Key.Name,
                           CompletedDone = b.Count()
                       })
                       .OrderByDescending(m => m.CompletedDone)
                       .ToListAsync();

            List<object> data = new List<object>();
            List<string> branchName = new List<string>();
            List<int> completedDone = new List<int>();

            foreach (var item in model)
            {
                branchName.Add(item.BranchName);
                completedDone.Add(item.CompletedDone);
            }
            data.Add(branchName);
            data.Add(completedDone);
            return data;
        }
        //المواعيد المتعثرة
        public async Task<List<Object>> UnfulfilledByBranchRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var model = await _context.Reservations.Include(m => m.Branches).Where(
                                m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date
                                && (m.Status == SD.BookingDone || m.Status == SD.DriverDone || m.Status == SD.DeleteDone))
                               .GroupBy(b => new { b.Branches.Name })
                               .Select(b => new DashboardViewModel()
                               {
                                   BranchName = b.Key.Name,
                                   Unfulfilled = b.Count()
                               })
                               .OrderByDescending(m => m.Unfulfilled)
                               .ToListAsync();

            List<object> data = new List<object>();
            List<string> branchName = new List<string>();
            List<int> unfulfilled = new List<int>();

            foreach (var item in model)
            {
                branchName.Add(item.BranchName);
                unfulfilled.Add(item.Unfulfilled);
            }
            data.Add(branchName);
            data.Add(unfulfilled);
            return data;
        }

        //المواعيد الملغية
        public async Task<List<Object>> MaxCancelByBranchRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var model = await _context.Reservations.Include(m => m.Branches)
                       .Where(m => m.AppointmentDate >= d1.Date
                               && m.AppointmentDate <= d2.Date
                               && m.Status == SD.CancelDone)
                               .GroupBy(b => new { b.Branches.Name })
                               .Select(b => new DashboardViewModel()
                               {
                                   BranchName = b.Key.Name,
                                   CancelDone = b.Count()
                               })
                               .OrderByDescending(m => m.CancelDone)
                               .ToListAsync();

            List<object> data = new List<object>();
            List<string> branchName = new List<string>();
            List<int> cancelDone = new List<int>();

            foreach (var item in model)
            {
                branchName.Add(item.BranchName);
                cancelDone.Add(item.CancelDone);
            }
            data.Add(branchName);
            data.Add(cancelDone);
            return data;
        }

        // المواعيد حسب الأحياء
        public async Task<List<Object>> MaxNeighboorhodRepo(string fromDate = null, string toDate = null)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var model = await _context.Reservations.Include(m => m.Customers).Include(m => m.Customers.Neighborhoods)
                                               .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date && m.Status != null)
                                               .GroupBy(b => new { b.Customers.Neighborhoods.Name })
                                               .Select(b => new DashboardViewModel()
                                               {
                                                   NeighName = b.Key.Name,
                                                   CountNeigh = b.Count()
                                               })
                                               .OrderByDescending(m => m.CountNeigh)
                                               .Take(5).ToListAsync();

            List<object> data = new List<object>();
            List<string> neighName = new List<string>();
            List<int> neighCount = new List<int>();

            foreach (var item in model)
            {
                neighName.Add(item.NeighName);
                neighCount.Add(item.CountNeigh);
            }

            data.Add(neighName);
            data.Add(neighCount);

            return data;
        }

        // المواعيد حسب السائقين
        public async Task<List<Object>> CountExuDriverRepo(string fromDate = null, string toDate = null)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var model = await _context.Reservations.Include(m => m.ApplicationUser)
                                               .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date
                                               && m.Status == SD.CompletedDone && m.DriverId != null)
                                               .GroupBy(b => new { b.ApplicationUserDriver.Name })
                                               .Select(b => new DashboardViewModel()
                                               {
                                                   DriverName = b.Key.Name,
                                                   CountDriverExu = b.Count()
                                               })
                                               .OrderByDescending(m => m.CountDriverExu)
                                               .Take(5)
                                               .ToListAsync();

            List<object> data = new List<object>();
            List<string> driverName = new List<string>();
            List<int> driverCount = new List<int>();

            foreach (var item in model)
            {
                driverName.Add(item.DriverName);
                driverCount.Add(item.CountDriverExu);
            }

            data.Add(driverName);
            data.Add(driverCount);

            return data;
        }


        //------------------------لوحة القيادة الثانية-------------------

        //عدد المتبرعين وعدد المواعيد
        public DashboardViewModel NumberoFDonorsRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var countDonors = _context.Customers.Count();
            var countAppo = _context.Reservations.Count();

            DashboardViewModel dashboardViewModel = new DashboardViewModel()
            {
                CountDonor = countDonors,
                CountAppo = countAppo
            };

            return dashboardViewModel;

        }

        // تقرير
        public async Task<List<DashboardViewModel>> TableReportRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var reservation = await _context.Reservations.Include(m => m.Branches)
                        .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date)
                        .ToListAsync();

            var grouped = from j in reservation
                          group j by new { j.Branches.Name }
                          into gr
                          select new DashboardViewModel
                          {
                              BranchName = gr.Key.Name,
                              BookingDone = gr.Count(c => c.Status == (SD.BookingDone)),
                              DriverDone = gr.Count(c => c.Status == (SD.DriverDone)),
                              CompletedDone = gr.Count(c => c.Status == (SD.CompletedDone)),
                              CancelDone = gr.Count(c => c.Status == (SD.CancelDone)),
                              DeleteDone = gr.Count(c => c.Status == (SD.DeleteDone)),
                              Total = gr.Count(),
                          };


            return grouped.OrderByDescending(m => m.Total).ToList();
        }

        //مقارنة المواعيد بالمواعيد المنفذه
        public async Task<List<Object>> ComparisonRepo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var reservation = await _context.Reservations.Include(m => m.Branches)
                       .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date)
                       .ToListAsync();

            var model = from j in reservation
                        group j by new { j.Branches.Name }
                        into gr
                        select new DashboardViewModel
                        {
                            BranchName = gr.Key.Name,
                            CompletedDone = gr.Count(c => c.Status == (SD.CompletedDone)),
                            Total = gr.Count()
                        };

            List<object> data = new List<object>();
            List<string> branchName = new List<string>();
            List<int> completedDone = new List<int>();
            List<int> total = new List<int>();

            foreach (var item in model)
            {
                branchName.Add(item.BranchName);
                completedDone.Add(item.CompletedDone);
                total.Add(item.Total);
            }
            data.Add(branchName);
            data.Add(completedDone);
            data.Add(total);
            return data;
        }

        //
        public async Task<List<DashboardViewModel>> GetDateRep(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var reservation = await _context.Reservations.Include(m => m.Branches)
                       .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date)
                       .ToListAsync();

            var model = from j in reservation

                        group j by new { j.Branches.Name }
                        into gr
                        select new DashboardViewModel
                        {
                            BranchName = gr.Key.Name,
                            Total = gr.Count()
                        };


            return model.ToList();
        }

        public async Task<List<Reservation_CustomerViewModel>> DetailsMaxBookingRpo(string fromDate, string toDate)
        {
            DateTime d1 = DateTime.Parse(fromDate);
            DateTime d2 = DateTime.Parse(toDate);

            var model = await _context.Reservations.Include(m => m.Branches)
                                                    .Include(m => m.Customers)
                                                    .Include(m => m.ApplicationUserDriver)
                                                    .Include(m => m.ApplicationUser)
                                                    .Include(m => m.Customers.Neighborhoods)
                       .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date)
                       .ToListAsync();

            var data = new List<Reservation_CustomerViewModel>();
            var res = new Reservation_CustomerViewModel();

            foreach (var item in model)
            {
                res = new Reservation_CustomerViewModel
                {
                    CustomerName = item.Customers.Name,
                    Mobily = item.Customers.Mobily,
                    Nieh = item.Customers.Neighborhoods.Name,
                    AppointmentDate = item.AppointmentDate.ToShortDateString(),
                    Priod = item.Priod,
                    Donation = item.Donation,
                    Comments = item.Comments,
                    Status = item.Status,
                    Notes = item.Notes,
                    // Driver = item.ApplicationUserDriver.Name,
                    Branches = item.Branches.Name,
                    UserName = item.ApplicationUser.Name
                };

                data.Add(res);
            }

            return data;
        }


    }
}

//public async Task<List<Object>> CountStatusRepo(string fromDate, string toDate)
//{
//    DateTime d1 = DateTime.Parse(fromDate);
//    DateTime d2 = DateTime.Parse(toDate);

//    var reservation = await _context.Reservations.Include(m => m.Branches)
//               .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date && m.Status != null)
//               .OrderByDescending(m => m.Status)
//               .ToListAsync();

//    var model = from j in reservation
//                group j by new { j.Branches.Id, j.Branches.Name }
//                into gr
//                select new DashboardViewModel
//                {
//                    BranchId = gr.Key.Id,
//                    BranchName = gr.Key.Name,
//                    BookingDone = gr.Count(c => c.Status == (SD.BookingDone)),
//                    DriverDone = gr.Count(c => c.Status == (SD.DriverDone)),
//                    CompletedDone = gr.Count(c => c.Status == (SD.CompletedDone)),
//                    CancelDone = gr.Count(c => c.Status == (SD.CancelDone)),
//                    DeleteDone = gr.Count(c => c.Status == (SD.DeleteDone)),
//                    Unfulfilled = gr.Count(c => c.Status == SD.BookingDone) +
//                                  gr.Count(c => c.Status == SD.DriverDone) +
//                                  gr.Count(c => c.Status == SD.DeleteDone),
//                    Total = gr.Count()
//                };

//    List<object> data = new List<object>();
//    List<string> branchName = new List<string>();
//    List<int> bookingDone = new List<int>();
//    List<int> driverDone = new List<int>();
//    List<int> completedDone = new List<int>();
//    List<int> cancelDone = new List<int>();
//    List<int> deleteDone = new List<int>();
//    List<int> unfulfilled = new List<int>();
//    List<int> total = new List<int>();

//    foreach (var item in model)
//    {
//        branchName.Add(item.BranchName);
//        bookingDone.Add(item.BookingDone);
//        driverDone.Add(item.DriverDone);
//        completedDone.Add(item.CompletedDone);
//        cancelDone.Add(item.CancelDone);
//        deleteDone.Add(item.DeleteDone);
//        unfulfilled.Add(item.Unfulfilled);
//        total.Add(item.Total);
//    }
//    data.Add(branchName);
//    data.Add(bookingDone);
//    data.Add(driverDone);
//    data.Add(completedDone);
//    data.Add(cancelDone);
//    data.Add(deleteDone);
//    data.Add(unfulfilled);
//    data.Add(total);
//    return data;
//}

////تقرير
//public async Task<List<DashboardViewModel>> CountAppoByStatusRepo(string fromDate, string toDate)
//{
//    DateTime d1 = DateTime.Parse(fromDate);
//    DateTime d2 = DateTime.Parse(toDate);

//    var reservation = await _context.Reservations.Include(m => m.Branches)
//                .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date && m.Status != null)
//                .ToListAsync();

//    var grouped = from j in reservation
//                  group j by new { j.Branches.Id, j.Branches.Name }
//                  into gr
//                  select new DashboardViewModel
//                  {
//                      BranchId = gr.Key.Id,
//                      BranchName = gr.Key.Name,
//                      BookingDone = gr.Count(c => c.Status == (SD.BookingDone)),
//                      DriverDone = gr.Count(c => c.Status == (SD.DriverDone)),
//                      CompletedDone = gr.Count(c => c.Status == (SD.CompletedDone)),
//                      CancelDone = gr.Count(c => c.Status == (SD.CancelDone)),
//                      Total = gr.Count(),
//                  };


//    return grouped.OrderBy(m => m.Total).ToList();
//}




//public async Task<List<DashboardViewModel>> CompletedAppointmentsRepo(string fromDate, string toDate)
//{
//    DateTime d1 = DateTime.Parse(fromDate);
//    DateTime d2 = DateTime.Parse(toDate);

//    var user = await _context.ApplicationUsers.Include(m => m.UserTypes).ToListAsync();
//    var groupedUser = from j in user
//                      group j by new { j.Id }
//                 into grUSer
//                      select new DashboardViewModel
//                      {
//                          TotalDriver = grUSer.Count(c => c.UserTypes.Type == ("سائق")),
//                          TotalUser = grUSer.Count(),
//                      };

//    var reservation = await _context.Reservations.Include(m => m.Branches)
//                .Where(m => m.AppointmentDate >= d1.Date && m.AppointmentDate <= d2.Date && m.Status != null)
//                .ToListAsync();

//    var grouped = from j in reservation
//                  group j by new { j.Branches.Id, j.Branches.Name }
//                  into gr
//                  select new DashboardViewModel
//                  {
//                      BranchId = gr.Key.Id,
//                      BranchName = gr.Key.Name,
//                      BookingDone = gr.Count(c => c.Status == (SD.BookingDone)),
//                      DriverDone = gr.Count(c => c.Status == (SD.DriverDone)),
//                      CompletedDone = gr.Count(c => c.Status == (SD.CompletedDone)),
//                      CancelDone = gr.Count(c => c.Status == (SD.CancelDone)),
//                      Total = gr.Count(),
//                      TotalDriver = groupedUser.Sum(c => c.TotalDriver),
//                      TotalUser = groupedUser.Count(),
//                      ReservationsList = gr
//                  };

//    if (reservation.Count() == 0)
//    {
//        List<DashboardViewModel> dataList = new List<DashboardViewModel>();

//        DashboardViewModel dashboardViewModel = new DashboardViewModel()
//        {
//            TotalDriver = groupedUser.Sum(c => c.TotalDriver),
//            TotalUser = groupedUser.Count()
//        };

//        dataList.Add(dashboardViewModel);

//        return dataList.ToList();
//    }

//    return grouped.OrderByDescending(m => m.Total).ToList();
//}