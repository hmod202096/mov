using Appointment.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IBranch_HistoryRepository
    {
        Task<List<Branch_HistoryDateViewModel>> GetAll(int brachId);
        Task<List<Branch_HistoryDateViewModel>> Edit(List<Branch_HistoryDateViewModel> branch_HistoriesList);
    }
}