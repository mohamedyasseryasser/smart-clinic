using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Patient;

namespace smart_clinic.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatient _patientService;

        public PatientController(IPatient patientService)
        {
            _patientService = patientService;
        }

        public async Task<IActionResult> Index(string? name, string? phone, long? nationalid, bool? isactive, int pageNumber = 1, int pageSize = 10)
        {
            var pg = new pagination { PageNumber = pageNumber, PageSize = pageSize };
            var response = await _patientService.GetAllAsync(pg, name, phone, nationalid, isactive);

            ViewBag.CurrentName = name;
            ViewBag.CurrentPhone = phone;
            ViewBag.CurrentNationalId = nationalid;
            ViewBag.CurrentIsActive = isactive;

            return View(response.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _patientService.GetPatientDetailsAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPatientVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var response = await _patientService.CreatePatient(vm);
            if (response.Success)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response.Message);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _patientService.getpatientbyid(id);
            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var updateVm = new UpdatePatientVM
            {
                patientid = response.Data.patientid,
                patientname = response.Data.patientname,
                phonenumber = response.Data.phonenumber,
                Gender = response.Data.Gender,
                datebirth = response.Data.datebirth,
                nationalid = response.Data.nationalid,
                isvalid = response.Data.isvalid
            };

            return View(updateVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePatientVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var response = await _patientService.UpdatePatient(vm);
            if (response.Success)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response.Message);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _patientService.RemovePatient(id);
            if (response.Success)
            {
                TempData["Success"] = "Patient deleted successfully";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
