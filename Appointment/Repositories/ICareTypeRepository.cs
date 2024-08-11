using Appointment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface ICareTypeRepository
    {
        Task<int> Add(CareTypes model);
        Task<CareTypes> Delete(CareTypes model);
        Task<CareTypes> Edit(CareTypes model);
        Task<List<CareTypes>> GetAll();
        Task<CareTypes> Find(int? id);
    }
}