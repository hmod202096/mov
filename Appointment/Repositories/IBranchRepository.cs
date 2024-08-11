using Appointment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IBranchRepository
    {
        Task<List<Branches>> GetAll();
        Task<int> AddBranch(Branches branches);
        Task<Branches> GetBranch(int? id);
        Task<Branches> EditBranch(Branches model);
        Task<Branches> DeleteBranch(Branches model);
        Task<int> AddBranchHistory(int id);
        Task<Branches> GetBranch(int branchId);
    }
}