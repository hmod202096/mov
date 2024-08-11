using Appointment.Contantes;
using Appointment.Data;
using Appointment.Enums;
using Appointment.Models;
using Appointment.Utility;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Appointment.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager, 
                                                         RoleManager<IdentityRole> roleManager,
                                                         ApplicationDbContext context)
        {
            var branchId = await DefaultBranch.SeedBranchAsync(context);
            var defaultUser = new ApplicationUser
            {
                UserName = "SuperAdmin",
                Email = "hhlr522@hotmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0554100522",
                PhoneNumberConfirmed = true,
                City = "Ryath",
                Name = "حمود الرشيدي",
                PasswordHash = "Ha@123456",
                BranchId = branchId,
                UserTypesId = ((int)TypeUserEnum.Manager)
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Ha@123456");
                    await userManager.AddToRoleAsync(defaultUser, SD.SuperAdminUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.BasicUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.AdminUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.ManagerUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.SupervisorUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.EmployeekUser.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, 
                                                    RoleManager<IdentityRole> roleManager,
                                                    ApplicationDbContext context)
        {
            var branchId = await DefaultBranch.SeedBranchAsync(context);
            var defaultUser = new ApplicationUser
            {
                UserName = "Basicuser",
                Email = "omar4580@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0544999595",
                PhoneNumberConfirmed = true,
                City = "Ryath",
                Name = "عمر العنزي",
                PasswordHash = "Ha@123456",
                BranchId = branchId,
                UserTypesId = ((int)TypeUserEnum.Manager),
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Ha@123456");
                    await userManager.AddToRoleAsync(defaultUser, SD.BasicUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.AdminUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.ManagerUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.SupervisorUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, SD.EmployeekUser.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }
      
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(SD.SuperAdminUser.ToString());
            await roleManager.AddPermissionClaim(adminRole, "Products");
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }


    }
}
