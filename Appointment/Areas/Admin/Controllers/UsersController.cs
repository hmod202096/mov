using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (SD.BasicUser + "," + SD.AdminUser))]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string searchName = null, string searchPhone = null, string searchEmail = null, string searchBranch = null)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            StringBuilder param = new StringBuilder();

            param.Append("&searchName");

            if (searchName != null)
            {
                param.Append(searchName);
            }
            else
            {
                searchName = "";
            }

            param.Append("&searchPhone");

            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            else
            {
                searchPhone = "";
            }

            param.Append("&searchEmail");

            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            else
            {
                searchEmail = "";
            }
            if (searchBranch != null)
            {
                param.Append(searchBranch);
            }
            else
            {
                searchBranch = "";
            }

            var users = await _userManager.Users
                .Include(m => m.UserTypes)
                .Include(m => m.Branch)
                .Where(m => m.Name.Contains(searchName) &&
                m.PhoneNumber.Contains(searchPhone) && 
                m.Branch.Name.Contains(searchBranch) && 
                m.Email.Contains(searchEmail))
                .OrderBy(m => m.Branch).ThenBy(m => m.Name)
                .Select(user => new UserViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserType = user.UserTypes.Type,
                    BranchName = user.Branch.Name,
                    LockoutEnd = user.LockoutEnd.Value,
                    Roles = _userManager.GetRolesAsync(user).Result,
                }).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> LouckUnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await context.ApplicationUsers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(100);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {

                ViewBag.ErrorMessage = $"Role with id = {id} cannot bo found";
                return View("NotFound");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userType = await context.UserTypes.ToListAsync();

            var model = new ApplicationUserModel()
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles,
                UserTypeId = user.UserTypesId,
                UserTypesList = userType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(ApplicationUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {model.Id} cannot bo found";
                return View("NotFound");
            }
            else
            {
                user.Name = model.Name;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UserTypesId = model.UserTypeId;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ManageUserRole(string userId)
        {
            ViewBag.UserId = userId;

            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with id = {userId} cannot bo found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                var userRoleViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.isSelected = true;
                }
                else
                {
                    userRoleViewModel.isSelected = false;
                }

                model.Add(userRoleViewModel);

            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageUserRole(List<UserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with id = {userId} cannot bo found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Connot Remove User existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.isSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Connot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("EditUser", new { id = userId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> TransferUser()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //string userId = claim.Value;

            List<ApplicationUser> applicationUsers = await context.ApplicationUsers
                                                                                 .Include(m => m.UserTypes)
                                                                                 .Where(m => m.EmailConfirmed == true)
                                                                                 .OrderBy(m => m.Name).ToListAsync();
            List<Branches> branchesList = await context.Branches.ToListAsync();

            UserViewModel userViewModel = new UserViewModel
            {
                ApplicationUsersList = applicationUsers,
                BranchesList = branchesList
            };

            return View(userViewModel);
        }

        public async Task<IActionResult> TransferEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {

                ViewBag.ErrorMessage = $"Role with id = {id} cannot bo found";
                return View("NotFound");
            }

            var model = new ApplicationUserModel()
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Email = user.Email,
                BranchId = user.BranchId,
                BranchesList = await context.Branches.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TransferEdit(ApplicationUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {model.Id} cannot bo found";
                return View("NotFound");
            }
            else
            {
                user.BranchId = model.BranchId;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("TransferUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

    }
}
