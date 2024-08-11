using Appointment.Models;
using Appointment.Repositories;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.Manager.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (SD.BasicUser))]
    public class CarTypesController : Controller
    {
        private readonly ICareTypeRepository _careTypeRepository;

        public CarTypesController(ICareTypeRepository careTypeRepository)
        {
            _careTypeRepository = careTypeRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _careTypeRepository.GetAll());
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CareTypes careTypes)
        {
            if (ModelState.IsValid)
            {
                var sav = await _careTypeRepository.Add(careTypes);

                if (sav > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(careTypes);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _careTypeRepository.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CareTypes model)
        {
            if (ModelState.IsValid)
            {
                var sav = await _careTypeRepository.Edit(model);

                if (sav.Id > 0)
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

            var model = await _careTypeRepository.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _careTypeRepository.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CareTypes model)
        {

            await _careTypeRepository.Delete(model);

            return RedirectToAction(nameof(Index));
        }



    }
}
