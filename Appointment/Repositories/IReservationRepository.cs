using Appointment.Models;
using Appointment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IReservationRepository
    {
        Task<BookingListViewModel> GetBooking(int branchId);
        Task<ReservtionViewModel> GetAll(int branchId, int pageNumber = 1, string searchName = null, string searchPhone = null);
        Task<ReservationOperationViewModel> CreateNew(int branchId, string userId, string mobily, DateTime dateTime);
        Task<int> AddReservation(ReservationOperationViewModel model);
        Task<bool> GetCountAppontment(int branchId, DateTime paramDate, int currentValue);
        Task<int> GetMaxDateRepository(int branchId);
        Task<ReservationDetailesViewModel> GetReservationDetails(int id);
        Task<Reservations> GetReservationStatus(int id);
        Task<Reservations> FindReservation(int? id);
        Task<ReservationOperationViewModel> GetResarvation(int? id);
        Task<ReservationOperationViewModel> EditReservation(ReservationOperationViewModel model);
        Task<Reservations> DeleteReservation(Reservations reservations);
        Task<List<SearchViewModel>> SearchRepository(string term);
        Task<List<ReservationAvailableViewModel>> AvailableRepository(int branchId);
        Task<List<ReservationByDateViewModel>> ViewReservationByDateRepository(string fromDate, int branchId);
        Task<List<DistributeViewModel>> ViewDistributionRepository(int branchId, string dat);
        Task<List<DistributeViewModel>> AddDistribute(List<DistributeViewModel> model);
        Task<List<DistributeViewModel>> ViewReservationForDriverRepository(string userId);
        Task<int> CompletedDone(int id);
        Task<OptionDeleteAppointmentViewModel> GetOptionDelete(int id, string d);
        Task<int> CancelReservationRepository(OptionDeleteAppointmentViewModel optionDeleteAppointmentViewModel);
        int UnfulfilledAppoRepo(int branchId);
        Task<UnfulfilledAppointmentViewModel> DetailesUnfulfilledAppo(int branchId);
        Task<int> DeleteUnfulfilledAppoRepo(int id);
    }
}