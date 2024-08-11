using Appointment.Data;
using Appointment.Enums;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Service;
using Appointment.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DriverRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        public async Task<UserViewModel> GetDriverUserByBranch(int branchId)
        {

            List<ApplicationUser> applicationUsers = await context.ApplicationUsers
                                            .Where(m => m.BranchId == branchId &&
                                            (m.LockoutEnd < DateTime.Now || m.LockoutEnd == null) &&
                                            m.UserTypesId == ((int)(TypeUserEnum.Driver)))
                                            .OrderBy(m => m.Name).ToListAsync();



            UserViewModel userViewModel = new UserViewModel
            {
                ApplicationUsersList = applicationUsers,
            };

            return userViewModel;
        }

        public async Task<ApplicationUserModel> SearchUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {

                return null;
            }

            var userType = await context.UserTypes.ToListAsync();

            var model = new ApplicationUserModel()
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Active = user.Active
            };

            return model;
        }

        public async Task<string> EditUser(ApplicationUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.Active = model.Active;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return user.Id;
            }

            return model.Id;
        }

    }
}
