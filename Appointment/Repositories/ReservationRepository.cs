using Appointment.Data;
using Appointment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointment.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Appointment.Service;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Appointment.Utility;
using Appointment.Contantes;
using Appointment.Enums;

namespace Appointment.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext context;

        public ReservationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<BookingListViewModel> GetBooking(int branchId)
        {
            BookingListViewModel reservtionViewModel = new BookingListViewModel
            {
                ReservationsList = await context.Reservations
                                .Include(m => m.Customers).Include(m => m.Customers.Neighborhoods)
                                .Where(m => m.BranchId == branchId
                                            && m.AppointmentDate >= DateTime.Now.Date)
                                .OrderBy(m=> m.AppointmentDate).ThenBy(m => m.Customers.Neighborhoods.Name)
                                .ToListAsync()
            };

            return reservtionViewModel;
        }

        public async Task<ReservtionViewModel> GetAll(int branchId, int pageNumber = 1, string searchName = null, string searchPhone = null)
        {
            StringBuilder param = new StringBuilder();

            param.Append("/Emplooye/Reservationes/Index?pageNumber=:");

            param.Append("&searchName");

            if (searchName != null)
            {
                param.Append(searchName);
            }
            else
            {
                searchName = "";
            }

            param.Append("&searchPhone");

            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            else
            {
                searchPhone = "";
            }


            ReservtionViewModel reservtionViewModel = new ReservtionViewModel
            {
                ReservationsList = await context.Reservations.Include(m => m.Customers).Include(m => m.Customers.Neighborhoods)
                                            .Where(m => m.BranchId == branchId
                                            && m.AppointmentDate >= DateTime.Now.Date
                                            && m.Customers.Name.Contains(searchName)
                                            && m.Customers.Mobily.Contains(searchPhone)).ToListAsync()
            };

            //Paginginfo

            var count = reservtionViewModel.ReservationsList.Count();

            int pageSize = 10;

            reservtionViewModel.ReservationsList = reservtionViewModel.ReservationsList.OrderBy(o => o.AppointmentDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            reservtionViewModel.Paginginfo = new Paginginfo()
            {
                CurrentPage = pageNumber,
                RecordsPerPage = pageSize,
                TotalRecords = count,
                UrlParam = param.ToString()
            };

            return reservtionViewModel;

        }

        public async Task<List<Neighborhoods>> GetNeighborhoodByBranchId(int branchId)
        {
            var data = await context.Branches_Neighborhoodes
                .Include(p => p.Branches)
                .Include(p => p.Neighborhoods)
                .Where(p => p.BranchId == branchId)
                .Select(p => new Branches_Neighborhoodes
                {
                    NeighborhoodId = p.NeighborhoodId,
                    Neighborhoods = p.Neighborhoods
                })
                .ToListAsync();

            var model = new List<Neighborhoods>();

            foreach (var item in data)
            {
                model.Add(new Neighborhoods()
                {
                    Id = item.Neighborhoods.Id,
                    Name = item.Neighborhoods.Name.ToString()
                }); ;
            }

            return model;

        }

        public async Task<List<Neighborhoods>> GetNeighborhoodInCustomer()
        {
            var data = await context.Neighborhoods.ToListAsync();

            var model = new List<Neighborhoods>();

            foreach (var item in data)
            {
                model.Add(new Neighborhoods()
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return model;

        }

        public async Task<ReservationOperationViewModel> CreateNew(int branchId, string userId, string mobily, DateTime dateTime)
        {

            //تم استخدام براميتر التاريخ للحصول على التاريخ من قائمة المواعيد المتاحة

            var customer = await context.Customers.Where(m => m.Mobily.Contains(mobily)).ToListAsync();

            if (customer.Count() > 0)
            {
                ReservationOperationViewModel model = new ReservationOperationViewModel()
                {
                    Customers = new Customers
                    {
                        Name = customer.FirstOrDefault().Name,
                        Mobily = customer.FirstOrDefault().Mobily,
                        NeighId = customer.FirstOrDefault().NeighId
                    },

                    Reservations = new Reservations
                    {
                        BranchId = branchId,
                        UserId = userId,
                        Priod = "1",
                        AppointmentDate = dateTime

                    },

                    Neighborhoods = await GetNeighborhoodInCustomer(),
                    MaxDate = await GetMaxDateRepository(branchId)
                };

                return model;
            }
            else
            {
                ReservationOperationViewModel model = new ReservationOperationViewModel()
                {
                    Customers = new Customers
                    {
                        Mobily = mobily
                    },

                    Reservations = new Reservations
                    {
                        BranchId = branchId,
                        UserId = userId,
                        Priod = "1",
                        AppointmentDate = Convert.ToDateTime(dateTime).Date
                    },

                    Neighborhoods = await GetNeighborhoodByBranchId(branchId),
                    MaxDate = await GetMaxDateRepository(branchId)
                };

                return model;
            }

        }

        // Save Date
        public async Task<int> AddReservation(ReservationOperationViewModel model)
        {
            //في حال أن المتبرع موجود مسبقاً

            var customer = await context.Customers.Where(m => m.Mobily.Contains(model.Customers.Mobily)).ToListAsync();

            if (customer.Count() > 0)
            {

                Reservations reservations = new Reservations();

                for (int i = 0; i < model.CountAppointment; i++)
                {
                    reservations = new Reservations()
                    {

                        AppointmentDate = model.Reservations.AppointmentDate,
                        Priod = model.Reservations.Priod,
                        Donation = model.Reservations.Donation,
                        Comments = model.Reservations.Comments,
                        CreateOn = DateTime.Now,
                        UpdateOn = DateTime.Now,
                        Status = SD.BookingDone,
                        BranchId = model.Reservations.BranchId,
                        CustId = customer.FirstOrDefault().Id,
                        UserId = model.Reservations.UserId
                    };

                    await context.Reservations.AddAsync(reservations);
                }

                await context.SaveChangesAsync();

                return reservations.Id;
            }
            else
            {
                //في حال أن المتبرع غير موجود 

                var newCustomer = new Customers()
                {
                    Name = model.Customers.Name,
                    Mobily = model.Customers.Mobily,
                    NeighId = model.Customers.NeighId
                };

                newCustomer.Reservations = new List<Reservations>();

                for (int i = 0; i < model.CountAppointment; i++)
                {
                    newCustomer.Reservations.Add(new Reservations()
                    {
                        AppointmentDate = model.Reservations.AppointmentDate,
                        Priod = model.Reservations.Priod,
                        Donation = model.Reservations.Donation,
                        Comments = model.Reservations.Comments,
                        CreateOn = DateTime.Now.Date,
                        UpdateOn = DateTime.Now.Date,
                        Status = SD.BookingDone,
                        BranchId = model.Reservations.BranchId,
                        CustId = newCustomer.Id,
                        UserId = model.Reservations.UserId
                    });

                    await context.Customers.AddAsync(newCustomer);
                }

                await context.SaveChangesAsync();

                return newCustomer.Id;
            }

        }

        //التحقق من الحجوزات المتاحة
        public async Task<bool> GetCountAppontment(int branchId, DateTime paramDate, int currentValue)
        {
            var countAppointment = await context.Reservations.Where(m => m.AppointmentDate.Date == paramDate && m.BranchId == branchId).CountAsync();

            var countAllow = await context.Branches_HistoryDates.Include(m => m.HistoryDate)
                                                     .Where(m => m.HistoryDate.Date == paramDate && m.BranchId == branchId).FirstOrDefaultAsync();

            var countAllow1 = await context.Branches_HistoryDates.Where(m => m.HistoryDate.Date == paramDate && m.BranchId == branchId).CountAsync();

            if (countAllow.CountBooking >= countAppointment + currentValue)
            {
                return true;
            }

            return false;
        }


        //الحد الأقصى للتاريخ 
        public async Task<int> GetMaxDateRepository(int branchId)
        {
            var maxDate = await context.Branches.FindAsync(branchId);

            return maxDate.MaxDate;
        }

        //تفاصيل الموعد
        // Cell mothed Ajax
        public async Task<ReservationDetailesViewModel> GetReservationDetails(int id)
        {

            ReservationDetailesViewModel reservationDetailesViewModel = new ReservationDetailesViewModel
            {
                Reservations = await context.Reservations.Include(m => m.Customers)
                                                         .Include(m => m.Customers.Neighborhoods)
                                                         .Include(m => m.Branches)
                                                         .Include(m => m.ApplicationUser)
                                                         .Include(m => m.ApplicationUserDriver)
                                                         .FirstOrDefaultAsync(m => m.Id == id)
            };



            return reservationDetailesViewModel;
        }

        //حالة الموعد
        // Cell mothed Ajax
        public async Task<Reservations> GetReservationStatus(int id)
        {
            Reservations reservations = await context.Reservations.Where(m => m.Id == id).FirstOrDefaultAsync();

            return reservations;
        }

        public async Task<Reservations> FindReservation(int? id)
        {
            var reservation = await context.Reservations.FindAsync(id);

            return reservation;
        }

        public async Task<ReservationOperationViewModel> GetResarvation(int? id)
        {
            var reservation = await FindReservation(id);

            ReservationOperationViewModel reservationOperationViewModel = new ReservationOperationViewModel()
            {
                Reservations = reservation,
                MaxDate = await GetMaxDateRepository(reservation.BranchId)
            };

            return reservationOperationViewModel;
        }

        public async Task<ReservationOperationViewModel> EditReservation(ReservationOperationViewModel model)
        {
            var reservation = await FindReservation(model.Reservations.Id);

            reservation.AppointmentDate = model.Reservations.AppointmentDate;
            reservation.Priod = model.Reservations.Priod;
            reservation.Donation = model.Reservations.Donation;
            reservation.Comments = model.Reservations.Comments;
            reservation.Status = SD.BookingDone;
            reservation.Notes = null;
            reservation.DriverId = null;

            context.Reservations.Update(reservation);
            await context.SaveChangesAsync();

            return model;
        }

        public async Task<Reservations> DeleteReservation(Reservations reservations)
        {
            context.Reservations.Remove(reservations);
            await context.SaveChangesAsync();
            return reservations;
        }

        //بحث
        public async Task<List<SearchViewModel>> SearchRepository(string term)
        {

            StringBuilder param = new StringBuilder();

            param.Append("&term");

            if (term != null)
            {
                param.Append(term);
            }
            else
            {
                term = "";
            }

            //group j by j.Customer.Name
            var reservation = await context.Reservations
                                .Include(x => x.Customers)
                                .Include(x => x.Customers.Neighborhoods)
                                .Include(x => x.Branches)
                                .Include(x => x.ApplicationUser)
                                .Include(x => x.ApplicationUserDriver)
                                .Where(m => m.Customers.Name == term || m.Customers.Mobily == term).OrderBy(m => m.Customers.Name).ToListAsync();

            var grouped = from j in reservation
                          group j by new { j.Customers.Name, j.Customers.Mobily, j.Customers.Neighborhoods }
                         into gr

                          select new SearchViewModel
                          {
                              Name = gr.Key.Name,
                              Mobily = gr.Key.Mobily,
                              Neighporhood = gr.Key.Neighborhoods.Name,
                              Reservations = gr
                          };

            return grouped.ToList();
        }

        //الحجوزات المتاحة 
        public async Task<List<ReservationAvailableViewModel>> AvailableRepository(int branchId)
        {
            int maxDate = await GetMaxDateRepository(branchId);

            //var model = await context.Branches_HistoryDates
            //           .Include(p => p.HistoryDate)
            //           .Where(p => p.BranchId == branchId && p.HistoryDate.Date >= DateTime.Now.Date)
            //           .OrderBy(p => p.HistoryDate.Date)
            //           .Select(x => new ReservationAvailableViewModel
            //           {
            //               Gregorian = x.HistoryDate.Date,
            //               Hijri = x.HistoryDate.Date.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("ar-SA")),
            //               DayName = x.HistoryDate.Date.ToString("dddd", new System.Globalization.CultureInfo("ar-SA")),
            //               CountBooking = x.CountBooking,
            //           }).Take(maxDate).ToListAsync();


            var model = await context.Branches_HistoryDates
                       .Include(p => p.HistoryDate)
                       .Where(p => p.BranchId == branchId && p.HistoryDate.Date >= DateTime.Now.Date)
                       .OrderBy(p => p.HistoryDate.Date)
                       .Take(maxDate)
                      .Select(x => new ReservationAvailableViewModel()
                      {
                          Gregorian = x.HistoryDate.Date,
                          CountBooking = x.CountBooking,
                      }).ToListAsync();


            foreach (var item in model)
            {
                item.Hijri = item.Gregorian.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("ar-SA"));
                item.DayName = item.Gregorian.ToString("dddd", new System.Globalization.CultureInfo("ar-SA"));
                item.Booked = GetCountBooking(branchId, item.Gregorian.Date).Result;
                item.Available = item.CountBooking - item.Booked;
            }

            return model;
        }

        //عدد الحجوزات
        public async Task<int> GetCountBooking(int branchId, DateTime date)
        {
            var count = await context.Reservations.CountAsync(x => x.BranchId == branchId && x.AppointmentDate.Date == date.Date);

            return count;
        }

        //عرض الحجوزات حسب التاريخ المحدد
        public async Task<List<ReservationByDateViewModel>> ViewReservationByDateRepository(string dat, int branchId)
        {
            if (dat == null)
            {
                dat = DateTime.Now.ToString();
            }

            DateTime d1 = DateTime.Parse(dat);

            var reservation = await context.Reservations
                                    .Include(m => m.Customers)
                                    .Include(m => m.Customers.Neighborhoods)
                                    .Include(m => m.ApplicationUser)
                                     .Include(m => m.ApplicationUserDriver)
                                    .Where(m => m.AppointmentDate == d1.Date && m.BranchId == branchId).ToListAsync();

            var model = new List<ReservationByDateViewModel>();

            string driverName = string.Empty;


            foreach (var item in reservation)
            {
                if (item.ApplicationUserDriver == null)
                {
                    driverName = null;
                }
                else
                {
                    driverName = item.ApplicationUserDriver.Name;
                }

                ReservationByDateViewModel reservationByDateViewModel = new ReservationByDateViewModel()
                {
                    Id = item.Id,
                    Name = item.Customers.Name,
                    Mobily = item.Customers.Mobily,
                    Neighborhood = item.Customers.Neighborhoods.Name,
                    AppointmentDate = item.AppointmentDate.Date,
                    Donation = item.Donation,
                    Comments = item.Comments,
                    DriverName = driverName,
                    Status = item.Status,
                    Notes = item.Notes,
                    UserName = item.ApplicationUser.Name
                };

                model.Add(reservationByDateViewModel);
            }
            return model;
        }

        //عرض توزيع المواعيد
        public async Task<List<DistributeViewModel>> ViewDistributionRepository(int branchId, string dat)
        {
            StringBuilder param = new StringBuilder();

            param.Append("&searchName");

            if (dat != null)
            {
                param.Append(dat);
            }
            else
            {
                dat = DateTime.Now.ToString();
            }

            DateTime dateTime = DateTime.Parse(dat);

            var reservations = await context.Reservations.Include(m => m.Customers)
                                                          .Include(m => m.Customers.Neighborhoods)
                                                          .Where(m => m.AppointmentDate.Date == dateTime.Date
                                                                                && m.BranchId == branchId
                                                                                && m.Status != SD.CompletedDone
                                                                                && m.Status != SD.CancelDone)
                                                          .OrderBy(m => m.Customers.Neighborhoods.Name)
                                                          .ToListAsync();

            var drivers = await context.ApplicationUsers
                        .Where(m => m.BranchId == branchId &&
                        m.Active == true &&
                        m.UserTypesId == ((int)TypeUserEnum.Driver)).ToListAsync();

            var maxDate = await GetMaxDateRepository(branchId);

            var model = new List<DistributeViewModel>();

            var distributeViewModel = new DistributeViewModel();

            if (reservations.Count > 0)
            {
                foreach (var item in reservations)
                {
                    distributeViewModel = new DistributeViewModel
                    {
                        Reservations = new Models.Reservations
                        {
                            Id = item.Id,
                            Customers = item.Customers,
                            AppointmentDate = item.AppointmentDate,
                            Donation = item.Donation,
                            Comments = item.Comments,
                            CreateOn = item.CreateOn,
                            UpdateOn = item.UpdateOn,
                            IsDelete = item.IsDelete,
                            CommentsDelete = item.CommentsDelete,
                            BranchId = item.BranchId,
                            CustId = item.CustId,
                            UserId = item.UserId,
                            Priod = item.Priod,
                            DriverId = item.DriverId,
                            Notes = item.Notes,
                            Status = item.Status
                        }
                    };

                    distributeViewModel.applicationUsersList = drivers;
                    distributeViewModel.MaxDate = maxDate;
                    model.Add(distributeViewModel);
                }
            }
            else
            {
                distributeViewModel = new DistributeViewModel
                {
                    MaxDate = maxDate
                };

                model.Add(distributeViewModel);
            }

            return model;
        }

        //حفظ توزيع المواعيد
        public async Task<List<DistributeViewModel>> AddDistribute(List<DistributeViewModel> model)
        {

            string optionStatus = string.Empty;

            for (int i = 0; i < model.Count(); i++)
            {
                if (model[i].Reservations.DriverId == null)
                {
                    optionStatus = SD.BookingDone;
                }
                else
                {
                    optionStatus = SD.DriverDone;
                }


                Reservations reservations = new Reservations
                {
                    Id = model[i].Reservations.Id,
                    AppointmentDate = model[i].Reservations.AppointmentDate,
                    Donation = model[i].Reservations.Donation,
                    Comments = model[i].Reservations.Comments,
                    CreateOn = model[i].Reservations.CreateOn,
                    UpdateOn = model[i].Reservations.UpdateOn,
                    IsDelete = model[i].Reservations.IsDelete,
                    CommentsDelete = model[i].Reservations.CommentsDelete,
                    BranchId = model[i].Reservations.BranchId,
                    CustId = model[i].Reservations.CustId,
                    UserId = model[i].Reservations.UserId,
                    Priod = model[i].Reservations.Priod,
                    DriverId = model[i].Reservations.DriverId,
                    Notes = model[i].Reservations.Notes,
                    Status = optionStatus,
                };

                context.Reservations.Update(reservations);
            }

            await context.SaveChangesAsync();

            return model;
        }

        //عرض المواعيد للسائق
        public async Task<List<DistributeViewModel>> ViewReservationForDriverRepository(string userId)
        {

            var model = await context.Reservations.Include(m => m.Customers)
                                                          .Include(m => m.Customers.Neighborhoods)
                                                          .Where(m => m.AppointmentDate.Date == DateTime.Now.Date &&
                                                                                (m.Status == SD.DriverDone ||
                                                                                m.Status == SD.CompletedDone ||
                                                                                m.Status == SD.CancelDone) &&
                                                                                m.DriverId == userId)
                                                          .Select(booking => new DistributeViewModel
                                                          {
                                                              Reservations = new Reservations
                                                              {
                                                                  Customers = new Customers
                                                                  {
                                                                      Name = booking.Customers.Name,
                                                                      Mobily = booking.Customers.Mobily,
                                                                      Neighborhoods = booking.Customers.Neighborhoods
                                                                  },

                                                                  Id = booking.Id,
                                                                  AppointmentDate = booking.AppointmentDate,
                                                                  Priod = booking.Priod,
                                                                  Donation = booking.Donation,
                                                                  Comments = booking.Donation,
                                                                  DriverId = booking.DriverId,
                                                                  Status = booking.Status,
                                                              }

                                                          }).ToListAsync();

            return model;
        }

        //تنفيذ الموعد
        public async Task<int> CompletedDone(int id)
        {

            var reservation = await context.Reservations.FindAsync(id);

            reservation.Status = SD.CompletedDone;
            reservation.Notes = "";

            context.Update(reservation);
            await context.SaveChangesAsync();

            return reservation.Id;
        }

        //سبب الحذف
        public async Task<OptionDeleteAppointmentViewModel> GetOptionDelete(int id, string d)
        {
            OptionDeleteAppointmentViewModel optionDeleteAppointment = new OptionDeleteAppointmentViewModel()
            {
                Id = id,
                Dat = d
            };

            return optionDeleteAppointment;
        }

        //الغاء الموعد 
        public async Task<int> CancelReservationRepository(OptionDeleteAppointmentViewModel optionDeleteAppointmentViewModel)
        {

            var data = await context.Reservations.FindAsync(optionDeleteAppointmentViewModel.Id);

            data.Status = SD.CancelDone;

            var notes = optionDeleteAppointmentViewModel.Notes;

            if (notes == "1")
            {
                data.Notes = "لم يتم الرد";
            }
            else if (notes == "2")
            {
                data.Notes = "تم تأجيل الموعد";
            }
            else
            {
                data.Notes = "الأثاث غير صالح للاستخدام";
            }

            context.Reservations.Update(data);
            await context.SaveChangesAsync();
            return data.Id;
        }

        //عدد المواعيد الغير منجزه
        public int UnfulfilledAppoRepo(int branchId)
        {
            var countAppointment = context.Reservations.Where(m => m.AppointmentDate.Date < DateTime.Now.Date && m.BranchId == branchId &&
                                    (m.Status == SD.BookingDone || m.Status == SD.DriverDone)).Count();
            return countAppointment;
        }

        //تفاصيل المواعيد الغير منجزه 
        public async Task<UnfulfilledAppointmentViewModel> DetailesUnfulfilledAppo(int branchId)
        {
            var model = await context.Reservations.Include(m => m.Customers).Include(m => m.Customers.Neighborhoods)
                                                                             .Where(m => m.AppointmentDate.Date < DateTime.Now.Date && m.BranchId == branchId
                                                                             && (m.Status == SD.BookingDone || m.Status == SD.DriverDone)).ToListAsync();

            Customers customers = new Customers();

            foreach (var item in model)
            {
                customers.Name = item.Customers.Name;
                customers.Mobily = item.Customers.Mobily;
                customers.Neighborhoods = item.Customers.Neighborhoods;
            }

            UnfulfilledAppointmentViewModel unfulfilledAppointmentViewModel  = new UnfulfilledAppointmentViewModel
            {
                customers = customers,
                reservationsList = model
            };

            return unfulfilledAppointmentViewModel;
        }

        //حذف الموعد الغير منجز
        public async Task<int> DeleteUnfulfilledAppoRepo(int id)
        {
            var reservation = await FindReservation(id);
            reservation.Status = SD.DeleteDone;
            context.Reservations.Update(reservation);
            await context.SaveChangesAsync();
            return reservation.Id;
        }

    }
}
