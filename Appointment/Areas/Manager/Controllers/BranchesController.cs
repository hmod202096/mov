using Appointment.Models;
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
    public class BranchesController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ISessionExtensions _sessionExtensions;
        public BranchesController(IBranchRepository branchRepository, ISessionExtensions sessionExtensions)
        {
            _branchRepository = branchRepository;
            _sessionExtensions = sessionExtensions;
        }
        public async Task<IActionResult> Generalization(bool isSuccess = false)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var branches = await _branchRepository.GetBranch(branch.Id);

            ViewBag.IsSuccess = isSuccess;

            return View(branches);
        }

        public async Task<IActionResult> MaxDate(bool isSuccess = false)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var branches = await _branchRepository.GetBranch(branch.Id);

            ViewBag.IsSuccess = isSuccess;

            return View(branches);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Branches branches, bool? generalization = false)
        {
            if (ModelState.IsValid)
            {
                var sav = await _branchRepository.EditBranch(branches);

                if (sav.Id > 0)
                {
                    if (generalization == true)
                    {
                        return RedirectToAction(nameof(Generalization), new { IsSuccess = true });
                    }
                    else
                    {
                        return RedirectToAction(nameof(MaxDate), new { IsSuccess = true });
                    }
                }
            }

            return View(branches);
        }
    }
}
