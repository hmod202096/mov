using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface IUserTypeReository
    {
        Task<List<UserTypes>> GetAll();
        Task<int> AddUserTypes(UserTypes userTypes);
        Task<UserTypes> GetUserTypes(int? id);
        Task<UserTypes> EditUserTypes(UserTypes userTypes);
        Task<UserTypes> DeleteUserTypes(UserTypes userTypes);
    }
}
