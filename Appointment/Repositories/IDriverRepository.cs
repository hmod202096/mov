using Appointment.Models;
using Appointment.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IDriverRepository
    {
        Task<UserViewModel> GetDriverUserByBranch(int branchId);
        Task<ApplicationUserModel> SearchUser(string id);
        Task<string> EditUser(ApplicationUserModel model);
    }
}