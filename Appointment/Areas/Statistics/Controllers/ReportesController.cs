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
    [Authorize(Roles = (SD.AdminUser))]
    public class ReportesController : Controller
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public ReportesController(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        //Get All Customer With Json
        public IActionResult CustometAll()
        {

            return View();
        }

        //View Appointment By Date 
        public async Task<IActionResult> AppointmentByDate(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            ViewBag.d1 = fromDate;
            ViewBag.d2 = toDate;

            var model = await _statisticsRepository.ReservationByDate(fromDate, toDate);

            return View(model);
        }

        //URL = /Statistics/Reportes/AllCustomerJson
        public async Task<IActionResult> AllCustomerJson()
        {
            var model = await _statisticsRepository.GetAllCustomer();

            return Json(new { data = model });
        }

        //عدد المواعيد حسب اسم المستخدم
        public async Task<IActionResult> CountReservationByUser(string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString();
                toDate = DateTime.Now.ToString();
            }

            ViewBag.d1 = fromDate;
            ViewBag.d2 = toDate;

            var model = await _statisticsRepository.CountReservationByUserRepo(fromDate, toDate);

            return View(model);
        }
    }
}
