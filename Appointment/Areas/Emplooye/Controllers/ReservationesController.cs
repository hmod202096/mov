using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Repositories;
using Appointment.Service;
using Appointment.Utility;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Areas.Emplooye.Controllers
{
    [Area("Emplooye")]
    public class ReservationesController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISessionExtensions _sessionExtensions;
        private readonly IConverter _converter;
        private readonly IUserService _userService;

        public ReservationesController(IReservationRepository reservationRepository,
                                        ISessionExtensions sessionExtensions,
                                        IConverter converter,
                                        IUserService userService)
        {
            _reservationRepository = reservationRepository;
            _sessionExtensions = sessionExtensions;
            _converter = converter;
            _userService = userService;
        }

        //قائمة المواعيد القائمة
        [Authorize(Roles = (SD.EmployeekUser))]
        public async Task<IActionResult> Index(int pageNumber = 1, string searchName = null, string searchPhone = null)
        {

            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var model = await _reservationRepository.GetAll(branch.Id, pageNumber, searchName, searchPhone);

            return View(model);
        }

        //حجز موعد
        [Authorize(Roles = (SD.EmployeekUser))]
        public async Task<IActionResult> Create(string mobily, DateTime dateTime)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);
            var userId = _sessionExtensions.GetUserId(HttpContext.Session);
            var data = await _reservationRepository.CreateNew(branch.Id, userId, mobily, dateTime);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationOperationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sav = await _reservationRepository.AddReservation(model);
                if (sav > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        //التحقق من عدد الحجوزات     URL : /Emplooye/Reservationes/GetCountAppontment
        [AllowAnonymous]
        public async Task<JsonResult> GetCountAppontment(DateTime paramDate, int currentValue)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var count = await _reservationRepository.GetCountAppontment(branch.Id, paramDate, currentValue);

            if (count == false)
            {
                return Json(new { success = false, message = "غير متـاح" });
            }

            return Json(new { success = true, message = "متـاح" });
        }

        //تفاصيل الموعد                     PartialView
        [AllowAnonymous]
        public async Task<IActionResult> GetReservationDetails(int id)
        {
            var model = await _reservationRepository.GetReservationDetails(id);

            return PartialView("_IndividualReservationDetailsPatrial", model);
        }

        //تفاصيل الموعد نموذج آخر           PartialView
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailsAppointment(int id)
        {
            var model = await _reservationRepository.GetReservationDetails(id);

            return PartialView("_DetailsAppointmentPartial", model);
        }

        //حالة الموعد                       PartialView
        [AllowAnonymous]
        public async Task<IActionResult> GetAppintmentStatus(int id)
        {
            var model = await _reservationRepository.GetReservationStatus(id);

            return PartialView("_AppointmentStatus", model.Status);
        }

        //تعديل بيانات الموعد
        [Authorize(Roles = (SD.EmployeekUser))]
        public async Task<IActionResult> Edit(int? id, bool isWhere = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationRepository.GetResarvation(id);

            if (reservation == null)
            {
                return NotFound();
            }

            ViewBag.IsWhere = isWhere;
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationOperationViewModel reservationOperationViewModel)
        {
            if (ModelState.IsValid)
            {
                var sav = await _reservationRepository.EditReservation(reservationOperationViewModel);

                if (sav.Reservations.Id > 0)
                {
                    if (reservationOperationViewModel.PageDetailesNotDoneAppointment == true)
                    {
                        return RedirectToAction(nameof(DetailesNotDoneAppointment));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(reservationOperationViewModel);
        }

        //حذف بيانات الموعد
        [Authorize(Roles = (SD.SupervisorUser))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationRepository.FindReservation(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Reservations reservations)
        {

            await _reservationRepository.DeleteReservation(reservations);

            return RedirectToAction(nameof(Index));
        }

        //الحجوزات المتاحة
        [Authorize(Roles = (SD.EmployeekUser))]
        public async Task<IActionResult> Available()
        {

            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var model = await _reservationRepository.AvailableRepository(branch.Id);

            return View(model);
        }

        //بحث
        [AllowAnonymous]
        public async Task<IActionResult> SearchInAppointment(string term, bool IsNull = true)
        {
            if (!string.IsNullOrEmpty(term))
            {
                ViewBag.IsNull = false;

                var model = await _reservationRepository.SearchRepository(term);

                return View(model);
            }

            ViewBag.IsNull = true;

            return View();
        }

        //توزيع المواعيد
        [Authorize(Roles = (SD.SupervisorUser))]
        public async Task<IActionResult> ViewDistribution(bool isSuccess = false, string date = null)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var model = await _reservationRepository.ViewDistributionRepository(branch.Id, date);

            ViewBag.IsSuccess = isSuccess;
            ViewBag.dat = date;

            return View(model);
        }

        //حفظ توزيع المواعيد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewDistribution(List<DistributeViewModel> model)
        {
            try
            {
                if (model.Count() > 0)
                {
                    var sav = await _reservationRepository.AddDistribute(model);

                    if (sav.Count > 0)
                    {
                        return RedirectToAction(nameof(ViewDistribution), new { IsSuccess = true });
                    }
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        //تسوية المواعيد
        [Authorize(Roles = (SD.ManagerUser + "," + SD.SupervisorUser))]
        public async Task<IActionResult> ViewReservationByDate(string dat = null)
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            if (string.IsNullOrEmpty(dat))
            {
                ViewBag.IsNull = true;
                ViewBag.dat = dat;
                return View();
            }
            else
            {
                var model = await _reservationRepository.ViewReservationByDateRepository(dat, branch.Id);
                ViewBag.IsNull = false;
                ViewBag.dat = dat;
                return View(model);
            }
        }

        //عرض المواعيد للسائق
        [Authorize(Roles = SD.DriverUser)]
        public async Task<IActionResult> ViewReservationForDriver()
        {
            var userId = _sessionExtensions.GetUserId(HttpContext.Session);

            var model = await _reservationRepository.ViewReservationForDriverRepository(userId);

            return View(model);
        }

        //تنفيذ الموعد
        [Authorize(Roles = SD.DriverUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> Completed(int id, bool reservationPage = false, string dat = null)
        {
            var sav = await _reservationRepository.CompletedDone(id);
            if (sav > 0)
            {
                //الذهاب لصفحة الحجوزات -- عرض المواعيد حسب التاريخ المحدد
                if (reservationPage == true)
                {
                    return RedirectToAction("ViewReservationByDate", new { dat });
                }
                return RedirectToAction(nameof(ViewReservationForDriver));
            }

            return NotFound();
        }

        //عرض خيارات الحذف          
        [Authorize(Roles = SD.DriverUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> GetDeleteAppointment(int id, string dat = null)
        {
            var model = await _reservationRepository.GetOptionDelete(id, dat);

            return PartialView("_OptionDeletePartial", model);
        }

        //الغاء الموعد
        [Authorize(Roles = SD.DriverUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> CancelReservation(OptionDeleteAppointmentViewModel optionDeleteAppointmentViewModel, string dat = null)
        {
            var sav = await _reservationRepository.CancelReservationRepository(optionDeleteAppointmentViewModel);
            if (sav > 0)
            {
                //الذهاب لصفحة الحجوزات -- عرض المواعيد حسب التاريخ المحدد
                if (!string.IsNullOrEmpty(dat))
                {
                    return RedirectToAction("ViewReservationByDate", new { dat });
                }

                return RedirectToAction(nameof(ViewReservationForDriver));
            }

            return NotFound();
        }

        //عرض المواعيد الغير منفذه
        [Authorize(Roles = (SD.SupervisorUser + "," + SD.ManagerUser))]
        public async Task<IActionResult> DetailesNotDoneAppointment()
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            var data = await _reservationRepository.DetailesUnfulfilledAppo(branch.Id);

            return View(data);
        }

        //حذف الموعد الغير منجز
        [Authorize(Roles = (SD.ManagerUser))]
        public async Task<IActionResult> DeleteUnfulfilledAppo(int id)
        {
            var data = await _reservationRepository.DeleteUnfulfilledAppoRepo(id);
            if (data > 0)
            {
                return RedirectToAction(nameof(DetailesNotDoneAppointment));
            }

            return null;
        }

        [AllowAnonymous]
        public IActionResult GeneratePdf(string html)
        {

            HtmlToPdf converter = new HtmlToPdf();
            html = html.Replace("start", "<").Replace("end", ">");
            PdfDocument doc = converter.ConvertHtmlString(html);
            byte[] pdfFile = doc.Save();
            doc.Close();
            return File(pdfFile, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> CreatePDF()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
                //Out = @"D:\PDFCreator\Employee_Report.pdf"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = await TemplateGenerator.GetHTMLString(_reservationRepository, _userService),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "صفحة [page] إلى [toPage]", Line = true },
                FooterSettings = { FontName = "Calibri", FontSize = 9, Line = true, Center = "جمعية البر" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);

            return File(file, "application/pdf");

            // return Ok("Successfully created PDF document.");
        }
    }
}
