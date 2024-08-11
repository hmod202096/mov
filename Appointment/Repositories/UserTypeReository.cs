using Appointment.Areas.Admin.Controllers;
using Appointment.Data;
using Appointment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class UserTypeReository : IUserTypeReository
    {
        private readonly ApplicationDbContext _context;

        public UserTypeReository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserTypes>> GetAll()
        {
            var data = await _context.UserTypes.ToListAsync();

            return data;
        }

        public async Task<int> AddUserTypes(UserTypes userTypes)
        {
            _context.UserTypes.Add(userTypes);
            await _context.SaveChangesAsync();
            return userTypes.Id;
        }

        public async Task<UserTypes> GetUserTypes(int? id)
        {
            var usertype = await _context.UserTypes.FindAsync(id);
            return usertype;
        }

        public async Task<UserTypes> EditUserTypes(UserTypes userTypes)
        {
            _context.UserTypes.Update(userTypes);
            await _context.SaveChangesAsync();
            return userTypes;
        }

        public async Task<UserTypes> DeleteUserTypes(UserTypes userTypes)
        {
            _context.UserTypes.Remove(userTypes);

            await _context.SaveChangesAsync();

            return userTypes;
        }

    }
}
