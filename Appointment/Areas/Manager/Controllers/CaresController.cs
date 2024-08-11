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

    public class CaresController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly ISessionExtensions _sessionExtensions;

        public CaresController(ICarRepository carRepository, ISessionExtensions sessionExtensions)
        {
            _carRepository = carRepository;
            _sessionExtensions = sessionExtensions;
        }

        public async Task<IActionResult> Index()
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var cares = await _carRepository.GetAll(branch.Id);
            return View(cares);
        }

        public async Task<IActionResult> Create()
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var model = await _carRepository.CreateNew(branch.Id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarAndCarTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sav = await _carRepository.AddCar(model);
                if (string.IsNullOrEmpty(sav.StatusMessage))
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var Car = await _carRepository.CreateNew(branch.Id);

            return View(Car);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _carRepository.GetCar(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarAndCarTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var upd = await _carRepository.EditCar(model);

                return RedirectToAction(nameof(Index));
            }

            var car = await _carRepository.GetCar(model.Cares.PlatId);

            return View(car);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _carRepository.GetCar(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _carRepository.GetCar(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CarAndCarTypeViewModel model)
        {
            await _carRepository.DeleteCar(model);

            return RedirectToAction(nameof(Index));
        }

    }
}
