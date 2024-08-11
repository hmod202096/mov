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
    public class Branch_HistoryesController : Controller
    {
        private readonly IBranch_HistoryRepository _branch_HistoryRepository;
        private readonly ISessionExtensions _sessionExtensions;

        public Branch_HistoryesController(IBranch_HistoryRepository branch_HistoryRepository, ISessionExtensions sessionExtensions)
        {
            _branch_HistoryRepository = branch_HistoryRepository;
            _sessionExtensions = sessionExtensions;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(bool success = false)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            ViewBag.Success = success;

            return View( await _branch_HistoryRepository.GetAll(branch.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(List<Branch_HistoryDateViewModel> model)
        {
            if (ModelState.IsValid)
            {
                var upd = await _branch_HistoryRepository.Edit(model);

                return RedirectToAction(nameof(Edit), new { success = true });
            }

            ModelState.AddModelError("", "حقول اجبارية");
            return View(model);
        }

    }
}
