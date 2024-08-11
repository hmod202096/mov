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

    public class Branch_NeighborhoodesController : Controller
    {
        private readonly IBranch_NeighborhoodRepository _branch_NeighborhoodRepository;
        private readonly ISessionExtensions _sessionExtensions;

        public Branch_NeighborhoodesController(IBranch_NeighborhoodRepository branch_NeighborhoodRepository, ISessionExtensions sessionExtensions)
        {
            _branch_NeighborhoodRepository = branch_NeighborhoodRepository;
            _sessionExtensions = sessionExtensions;
        }

        public async Task<IActionResult> Edit(bool isSuccess = false)
        {

            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var data = await _branch_NeighborhoodRepository.GetNeighborhood(branch.Id);

            ViewBag.IsSuccess = isSuccess;

            return View(data);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<Branch_NeighborhoodViewModel> model)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            await _branch_NeighborhoodRepository.EditNeighborhoodInBranch(branch.Id, model);

            return RedirectToAction("Edit", new { IsSuccess = true });

        }

    }
}
