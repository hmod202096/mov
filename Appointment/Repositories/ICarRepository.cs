using Appointment.Models;
using Appointment.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface ICarRepository
    {
        Task<List<Cares>> GetAll(int branchId);
        Task<CarAndCarTypeViewModel> CreateNew(int branchId);
        Task<CarAndCarTypeViewModel> AddCar(CarAndCarTypeViewModel model);
        Task<CarAndCarTypeViewModel> GetCar(string id);
        Task<CarAndCarTypeViewModel> EditCar(CarAndCarTypeViewModel model);
        Task<Cares> DeleteCar(CarAndCarTypeViewModel model);
    }
}