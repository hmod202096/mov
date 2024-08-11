using Appointment.Repositories;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.Statistics.Controllers
{
    [Area("Statistics")]
    [Authorize(Roles = SD.AdminUser)]

    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        
        public async Task<IActionResult> Index(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            ViewBag.d1 = fromDate;
            ViewBag.d2 = toDate;

            var model = await _dashboardRepository.CountUsersRepo(fromDate, toDate);

            return View(model);
        }

        public async Task<IActionResult> CountUsers(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var model = await _dashboardRepository.CountUsersRepo(fromDate, toDate);

            return View(model);
        }

        public async Task<List<object>> AllOperations(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = await _dashboardRepository.AllOperationsRepo(fromDate, toDate);

            return data;
        }

        public async Task<List<object>> MaxBookingByBranch(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = await _dashboardRepository.MaxBookingByBranchRepo(fromDate, toDate);

            return data;
        }

        public async Task<List<object>> MaxCompletedByBranch(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = await _dashboardRepository.MaxCompletedByBranchRepo(fromDate, toDate);

            return data;
        }

        public async Task<List<object>> UnfulfilledByBranch(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = await _dashboardRepository.UnfulfilledByBranchRepo(fromDate, toDate);

            return data;
        }

        public async Task<List<object>> MaxCancelByBranch(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = await _dashboardRepository.MaxCancelByBranchRepo(fromDate, toDate);

            return data;
        }

        public Task<List<object>> GetMaxNiegh(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = _dashboardRepository.MaxNeighboorhodRepo(fromDate, toDate);

            return data;
        }

        public Task<List<object>> CountExuDriver(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = _dashboardRepository.CountExuDriverRepo(fromDate, toDate);

            return data;
        }

       public async Task<IActionResult> DetailsMaxBooking(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var model = await _dashboardRepository.DetailsMaxBookingRpo(fromDate, toDate);

            return PartialView("_DetailsAppChart", model.OrderByDescending(m=> m.Branches));
        }
    }
}
