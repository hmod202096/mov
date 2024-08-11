using Appointment.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IBranch_NeighborhoodRepository
    {
        Task<List<Branch_NeighborhoodViewModel>> GetNeighborhood(int branchId);
        Task<List<SelectListItem>> GetNeighborhoodByBranchId(int branchId);
        Task EditNeighborhoodInBranch(int branchId, List<Branch_NeighborhoodViewModel> model);
        Task<bool> GetBranchIdNeighborhoodId(int branchId, int neighborhoodId);
    }
}