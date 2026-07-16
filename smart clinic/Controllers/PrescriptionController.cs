using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.prescription;
using smart_clinic.viewmodels.prescriptionitems;

namespace smart_clinic.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly ILogger<Prescription> logger;
        private readonly IPrescription _prescriptionService;
        private readonly IMedicine _medicineService;
        private readonly IVisit _visitService;

        public PrescriptionController(ILogger<Prescription> logger,IPrescription prescriptionService, IMedicine medicineService, IVisit visitService)
        {
            this.logger = logger;
            _prescriptionService = prescriptionService;
            _medicineService = medicineService;
            _visitService = visitService;
        }
        // GET: Prescription
        public async Task<IActionResult> Index(DateTime? date = null, int pageNumber = 1, int pageSize = 10)
        {
            var pg = new pagination { PageNumber = pageNumber, PageSize = pageSize };
            var response = await _prescriptionService.GetAllPrescriptions(pg, date ?? DateTime.Now);
            if (response.Success)
            {
                ViewBag.date = date;
                return View(response.Data);
            }
            return NotFound();
        }

        // GET: Prescription/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _prescriptionService.GetPrescriptionById(id);

            if (response.Success)
            {
                return View(response.Data);
            }
            logger.LogWarning(response.Message);
            return NotFound();
        }

        // GET: Prescription/Create
        public async Task<IActionResult> Create(int visitid)
        {
            await PopulateDropdowns();
 
            return View(new AddPrescriptionVM { visitid=visitid , items = new List<AddPrescriptionItemVM>() });
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPrescriptionVM vm)
        {
            if (ModelState.IsValid)
            {
                var response = await _prescriptionService.CreatePrescription(vm);
                if (response.Success)
                {
                     return RedirectToAction(nameof(Index));
                }
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            await PopulateDropdowns();
            return View(vm);
        }

        // GET: Prescription/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _prescriptionService.GetPrescriptionById(id);
            if (response.Success)
            {
                var updateVm = new UpdatePrescriptionvm
                {
                    prescriptionid = response.Data.prescriptionid,
                    prescriptiondate = response.Data.prescriptiondate,
                    notes = response.Data.notes,
                    visitid=response.Data.visitid,
                     items = response.Data.prescriptionitems.Select(item => new UpdatePrescriptionitemvm
                    {
                        prescriptionitemid = item.prescriptionitemid,
                        quantity = item.quantity,
                        Dosage = item.Dosage,
                        Frequency = item.Frequency,
                        Duration = item.Duration,
                        notes = item.notes,
                       prescriptionid = item.prescriptionid,
                        mdeicineid = item.mdeicineid
                    }).ToList()
                };
                await PopulateDropdowns();
                return View(updateVm);
            }
            return NotFound();
        }

        // POST: Prescription/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePrescriptionvm vm)
        {
            if (ModelState.IsValid)
            {
                var response = await _prescriptionService.UpdatePrescription(vm);

                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            await PopulateDropdowns();
            return View(vm);
        }

        // GET: Prescription/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _prescriptionService.GetPrescriptionById(id);
            if (response.Success)
            {

                return View(response.Data);
            }
            logger.LogWarning(response.Message);

            return NotFound();
        }

        // POST: Prescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _prescriptionService.DeletePrescription(id);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            // Handle error
            return View("Error", response.Errors);
        }

        private async Task PopulateDropdowns()
        {
            var medicinesResponse = await _medicineService.getactivemedicine(); // Assuming GetAllMedicines exists
            if (medicinesResponse.Success)
            {
                ViewBag.Medicines = new SelectList(medicinesResponse.Data, "medicineId", "Name");
            }
            else
            {
                ViewBag.Medicines = new SelectList(new List<object>(), "medicineId", "Name");
            }
 
        }
    }
}
