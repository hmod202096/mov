using Appointment.Models;
using System.Threading.Tasks;

namespace Appointment.Service
{
    public interface IUserService
    {
        string GetUserId();
        Task<ApplicationUser> GetUser();
        Task<Branches> GetBranch();


    }
}