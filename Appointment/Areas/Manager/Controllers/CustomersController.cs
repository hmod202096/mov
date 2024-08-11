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
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISessionExtensions _sessionExtensions;

        public CustomersController(ICustomerRepository customerRepository, ISessionExtensions sessionExtensions)
        {
            _customerRepository = customerRepository;
            _sessionExtensions = sessionExtensions;
        }
       
        public async Task<IActionResult> Index(string Mobily, bool sav = false)
        {
            if (!string.IsNullOrEmpty(Mobily))
            {
                ViewBag.IsNull = false;
            }
            else
            {
                ViewBag.IsNull = true;
            }

            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var data = await _customerRepository.SearchByMobily(Mobily, branch.Id, sav);

            return View(data);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                var data = await _customerRepository.EditCustomer(customer);

                if (data > 0)
                {
                    return RedirectToAction(nameof(Index), new { sav = true});
                }
            }

            return View(customer);
        }

    }
}
