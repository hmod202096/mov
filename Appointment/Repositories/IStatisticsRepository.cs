using Appointment.Models;
using Appointment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IStatisticsRepository
    {
        Task<List<StatisticsViewModel>> ReservationByDate(string fromDate, string toDate);
        Task<List<Customers>> GetAllCustomer();
        Task<List<CountAppByUserViewModel>> CountReservationByUserRepo(string fromDate = null, string toDate = null);
    }
}
