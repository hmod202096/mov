using Appointment.Models;
using Appointment.Repositories;
using Appointment.Service;
using Appointment.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Appointment.Areas.Emplooye.Controllers
{
    [Area("Emplooye")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ISessionExtensions _sessionExtensions;

        public HomeController(IBranchRepository branchRepository, ISessionExtensions sessionExtensions)
        {
            _branchRepository = branchRepository;
            _sessionExtensions = sessionExtensions;
        }

        public async Task<IActionResult> Index()
        {
            var branch = await _sessionExtensions.GetBranch(HttpContext.Session);

            //عرض الرسالة 
            ViewBag.Message = branch.Message;

            //عدد المواعيد الغير منجزه 
            var UnfulfilledAppo = _sessionExtensions.GetCountUnfulfilledAppo(HttpContext.Session);

            return View();
        }

        [Route("/Emplooye/Home/Error/{Code:int}")]
        public IActionResult Error(int code)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, 
                                             ErrorMessage = $"Error Occured. Error Code is {code}"});

        }
    }
}
