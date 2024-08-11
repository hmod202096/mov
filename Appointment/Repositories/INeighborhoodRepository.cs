using Appointment.Models;
using Appointment.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface INeighborhoodRepository
    {
        Task<NeighborhoodViewModel> GetAll(int pageNumber, string searchName = null);
        Task<NeighborhoodOperationViewModel> AddNeighborhood(NeighborhoodOperationViewModel neighborhoodCreateViewModel);
        Task<NeighborhoodOperationViewModel> GetNeighborhood(int? id);
        Task<NeighborhoodOperationViewModel> EditNeighborhood(NeighborhoodOperationViewModel model);
        Task<NeighborhoodOperationViewModel> DeleteNeighborhood(NeighborhoodOperationViewModel model);
    }
}