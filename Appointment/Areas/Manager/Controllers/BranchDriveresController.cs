using Appointment.Models.ViewModel;
using Appointment.Repositories;
using Appointment.Service;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = SD.ManagerUser)]
    public class BranchDriveresController : Controller
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ISessionExtensions _sessionExtensions;

        public BranchDriveresController(IDriverRepository driverRepository,
                                        ISessionExtensions sessionExtensions)
        {
            _driverRepository = driverRepository;
            _sessionExtensions = sessionExtensions;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _sessionExtensions.GetUser(HttpContext.Session);

            var data = await _driverRepository.GetDriverUserByBranch(user.BranchId);

            return View(data);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _driverRepository.SearchUser(id);

            if (driver == null)
            {

                ViewBag.ErrorMessage = $"Role with id = {id} cannot bo found";
                return View("NotFound");
            }


            return View(driver);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserModel model)
        {
            var result = await _driverRepository.EditUser(model);

            if (!string.IsNullOrEmpty(result))
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}
