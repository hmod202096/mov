using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Repositories;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (SD.BasicUser))]
    public class NeighborhoodsController : Controller
    {
        private readonly INeighborhoodRepository _neighborhoodRepository;

        public NeighborhoodsController(INeighborhoodRepository neighborhoodRepository)
        {
            _neighborhoodRepository = neighborhoodRepository;
        }
       
        public async Task<IActionResult> Index(int pageNumber = 1, string searchName = null)
        {
            return View(await _neighborhoodRepository.GetAll(pageNumber, searchName));
        }

        public IActionResult Create()
        {
            NeighborhoodOperationViewModel neighborhoodCreateViewModel = new NeighborhoodOperationViewModel
            {
                Neighborhoods = new Neighborhoods()
            };

            return View(neighborhoodCreateViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(NeighborhoodOperationViewModel neighborhoodCreateViewModel)
        {
            if (ModelState.IsValid)
            {
              var sav = await _neighborhoodRepository.AddNeighborhood(neighborhoodCreateViewModel);

                if (sav.Neighborhoods.Id > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(neighborhoodCreateViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighborhood = await _neighborhoodRepository.GetNeighborhood(id) ;

            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NeighborhoodOperationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var upd = await _neighborhoodRepository.EditNeighborhood(model);

                if (string.IsNullOrEmpty(model.StatusMessage))
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighborhood = await _neighborhoodRepository.GetNeighborhood(id);

            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighborhood = await _neighborhoodRepository.GetNeighborhood(id);

            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(NeighborhoodOperationViewModel model)
        {
            
            await _neighborhoodRepository.DeleteNeighborhood(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
