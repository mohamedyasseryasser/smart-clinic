using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_clinic.filter;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.home;
using System.Diagnostics;
using System.Threading.Tasks;

namespace smart_clinic.Controllers
{
    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUser userservice;
        private readonly IPatient patientservice;
        private readonly IAppoinment appoinmentservice;

        public HomeController(
            ILogger<HomeController> logger,
            IUser userservice,
            IPatient patientservice,
            IAppoinment appoinmentservice)
        {
            _logger = logger;
            this.userservice = userservice;
            this.patientservice = patientservice;
            this.appoinmentservice = appoinmentservice;
        }
       
        public async Task<IActionResult> Index()
        {
            var doctorcount = await userservice.getadoctorcount();
            var patientcount = await patientservice.getpatientcount();

            var appointmentResponse =
                await appoinmentservice.getallappoinment(
                    DateTime.Today,
                    new pagination
                    {
                        PageNumber = 1,
                        PageSize = 5
                    });

            var vm = new dashboardvm
            {
                totaldoctors = doctorcount.Data,
                patientcount = patientcount.Data,
                appoinmentcount = appointmentResponse.Data.Count(),
                items = appointmentResponse.Data.ToList()
            };

            return View(vm);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
