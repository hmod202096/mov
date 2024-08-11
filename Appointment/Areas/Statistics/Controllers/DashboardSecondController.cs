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
    public class DashboardSecondController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardSecondController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        //عدد المتبرعين وعدد المواعيد
        public IActionResult Index(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            ViewBag.d1 = fromDate;
            ViewBag.d2 = toDate;

            var model = _dashboardRepository.NumberoFDonorsRepo(fromDate, toDate);

            return View(model);

        }

        public async Task<JsonResult> TableReport(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var model = await _dashboardRepository.TableReportRepo(fromDate, toDate);

            return Json(model);
        }

        public async Task<List<object>> Comparison(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = await _dashboardRepository.ComparisonRepo(fromDate, toDate);

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

        public async Task<JsonResult> GetData(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            var data = await _dashboardRepository.GetDateRep(fromDate, toDate);

            return Json(data);
        }

    }
}
