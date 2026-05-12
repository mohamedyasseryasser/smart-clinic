using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smart_clinic.enums;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.Appoinment;
using smart_clinic.viewmodels.General;
using System.Threading.Tasks;

namespace smart_clinic.Controllers
{
    [Authorize]
    public class AppoinmentController : Controller
    {
        private readonly IAppoinment _appoinmentService;
        private readonly IPatient _patientService;
        private readonly ILogger<AppoinmentController> logger;
        private readonly IUser userservice;
        private readonly IVisit visitservice;

        public AppoinmentController(IAppoinment appoinmentService,
            IPatient patientService,
            ILogger<AppoinmentController> logger,IUser userservice,IVisit visitservice)
        {
            _appoinmentService = appoinmentService;
            _patientService = patientService;
            this.logger = logger;
            this.userservice = userservice;
            this.visitservice = visitservice;
        }

        // GET: Appoinment/Search
        public IActionResult Search()
        {
            return View();
        }
        // POST: Appoinment/Search
        [HttpPost]
        public async Task<IActionResult> Search(SearchAppoinmentVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
 
            var result = await _appoinmentService.SearchAppoinment(vm);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(vm);
            }

            return View("SearchResults", result.Data);
        }
        private async Task loaddoctorsandreceptionists(AddAppoinmentVM vm)
        {
           
            //load doctors dropdownlist
            var doctors =await userservice.getdoctors();
            vm.doctorsvm = doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorId.ToString(),
                Text = d.user.UserName
            }).ToList();
            //load receptionists dropdownlist
            var rescptionist = await userservice.getreceptionists();
            vm.receptionists = rescptionist.Select(d => new SelectListItem
            {
                Value = d.resptionistid.ToString(),
                Text = d.user.UserName
            }).ToList();
           
        }
        // GET: Appoinment/Create
        public async Task<IActionResult> Create()
        {
            var vm = new AddAppoinmentVM();
            await loaddoctorsandreceptionists(vm);
            return View(vm);
        }
        // POST: Appoinment/Create
        [HttpPost]
        public async Task<IActionResult> Create(AddAppoinmentVM vm)
        {
             
                //check validation
                if (!ModelState.IsValid)
                {
                    logger.LogWarning("Invalid addapoinmentvm attempt (ModelState invalid)");
                    return View(vm);
                }
                var result = await _appoinmentService.CreateAppoinment(vm);
                if (!result.Success)
                {
                    logger.LogWarning($"Create appointment failed: {result.Message}");
                    //check patient
                    if (!string.IsNullOrEmpty(vm.PhoneNumber))
                    {
                        var check = await _patientService.getpatientbyphonenumber(vm.PhoneNumber);
                        if (!check.Success)
                        {
                            logger.LogWarning($"phone number is not found:{result.Message}");
                            return RedirectToAction("Create", "Patient");
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    await loaddoctorsandreceptionists(vm);
                    return View(vm);
                }
                return RedirectToAction("Details", new { id = result.Data.appoimentid });
            
           
            }
        // GET: Appoinment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _appoinmentService.GetAppoinmentById(id);
            if (!result.Success)
            {
                logger.LogWarning($"this appoinmentid {id}is not found");
                return NotFound();
            }

            return View(result.Data);
        }
        // GET: Appoinment/ConfirmArrival/5
        public async Task<IActionResult> ConfirmArrival(int id)
        {
            var result = await _appoinmentService.GetAppoinmentById(id);
            if (!result.Success)
            {
                logger.LogWarning($"this appoinmentid {id}is not found");
                return NotFound();
            }
            var patient = await _patientService.getpatientbyphonenumber(result.Data.PhoneNumber);
            var vm = new UpdateAppoinmentStateVM 
            {
             phone=result.Data.PhoneNumber,
             patientname=patient.Data.patientname,
             AppoinmentId=result.Data.appoimentid,
             Status=AppointmentStatus.Confirmed,
            };
            return View(vm);
        }
        // POST: Appoinment/ConfirmArrival/5
        [HttpPost]
        public async Task<IActionResult> ConfirmArrival(UpdateAppoinmentStateVM vm)
        {
            var result = await _appoinmentService.UpdateAppoinmentState(vm);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(result.Data);
            }

            TempData["SuccessMessage"] = "تم تأكيد حضور المريض.";
            return RedirectToAction("Details", new { id = result.Data.appoimentid });
        }
        // GET: Appoinment/StartVisit/5
        public async Task<IActionResult> StartVisit(int id)
        {
             
            var result = await _appoinmentService.GetAppoinmentById(id);
            if (!result.Success)
            {
                return NotFound();
            }
            if (result.Data.status != AppointmentStatus.Confirmed)
            {
                TempData["ErrorMessage"] = "must attend patient first";
                return RedirectToAction("Details", new { id = id });
            }
            return View(result.Data);
        }
        // GET: Appoinment/Cancel/5
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _appoinmentService.GetAppoinmentById(id);
            if (!result.Success)
            {
                return NotFound();
            }
            return View(result.Data);
        }

        // POST: Appoinment/Cancel/5
        [HttpPost]
        public async Task<IActionResult> Cancele(int id)
        {
            var result = await _appoinmentService.CancelAppoinment(id);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(result.Data);
            }
            TempData["SuccessMessage"] = "appoinment canceled";
            return RedirectToAction("Search");
        }
    }
}
