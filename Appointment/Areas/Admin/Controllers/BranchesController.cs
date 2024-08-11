using Appointment.Models;
using Appointment.Repositories;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (SD.BasicUser))]
    public class BranchesController : Controller
    {
        private readonly IBranchRepository _branchRepository;

        public BranchesController(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }
        public async Task<IActionResult> Index()
        {
            var branches = await _branchRepository.GetAll();

            return View(branches);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branches branches)
        {
            if (ModelState.IsValid)
            {
                var sav = await _branchRepository.AddBranch(branches);

                if (sav > 0)
                {
                    await _branchRepository.AddBranchHistory(sav);

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(branches);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _branchRepository.GetBranch(id);

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Branches branches)
        {
            if (ModelState.IsValid)
            {
                var sav = await _branchRepository.EditBranch(branches);

                if (sav.Id > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(branches);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _branchRepository.GetBranch(id);

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _branchRepository.GetBranch(id);

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Branches model)
        {
            await _branchRepository.DeleteBranch(model);

            return RedirectToAction(nameof(Index));
        }


    }
}
