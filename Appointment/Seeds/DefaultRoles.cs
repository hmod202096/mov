using Appointment.Contantes;
using Appointment.Enums;
using Appointment.Models;
using Appointment.Utility;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(SD.SuperAdminUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.SuperAdminUser.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(SD.BasicUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.BasicUser.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(SD.AdminUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.AdminUser.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(SD.ManagerUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.ManagerUser.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(SD.SupervisorUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.SupervisorUser.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(SD.EmployeekUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.EmployeekUser.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(SD.DriverUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.DriverUser.ToString()));
            }
            if (!await roleManager.RoleExistsAsync(SD.CustomerUser.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.CustomerUser.ToString()));
            }

        }

    }
}
