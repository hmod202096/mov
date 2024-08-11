using Appointment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IDashboardRepository
    {
        Task<List<DashboardViewModel>> CountUsersRepo(string fromDate, string toDate);
        Task<List<Object>> AllOperationsRepo(string fromDate, string toDate);
        Task<List<Object>> MaxBookingByBranchRepo(string fromDate, string toDate);
        Task<List<Object>> MaxCompletedByBranchRepo(string fromDate, string toDate);
        Task<List<Object>> UnfulfilledByBranchRepo(string fromDate, string toDate);
        Task<List<Object>> MaxCancelByBranchRepo(string fromDate, string toDate);
        Task<List<Object>> MaxNeighboorhodRepo(string fromDate = null, string toDate = null);
        Task<List<Object>> CountExuDriverRepo(string fromDate = null, string toDate = null);

        //لوحة القيادة الثانية
        DashboardViewModel NumberoFDonorsRepo(string fromDate, string toDate);
        Task<List<DashboardViewModel>> TableReportRepo(string fromDate, string toDate);
        Task<List<Object>> ComparisonRepo(string fromDate, string toDate);

        Task<List<DashboardViewModel>> GetDateRep(string fromDate, string toDate);

        Task<List<Reservation_CustomerViewModel>> DetailsMaxBookingRpo(string fromDate, string toDate);

    }
}