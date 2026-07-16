using Microsoft.AspNetCore.Mvc;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Visit;
using smart_clinic.viewmodels.VisitRepo;
using System.Threading.Tasks;

namespace smart_clinic.Controllers
{

    public class VisitController : Controller
    {
        public VisitController(IVisit visitservices)
        {
            Visitservices = visitservices;
        }

        public IVisit Visitservices { get; }

        public async Task<IActionResult> Index(DateTime? date, int pageNumber = 1, int pageSize = 10)
        {
            var selectedDate = date ?? DateTime.Today;

            var pg = new pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await Visitservices.getallvisit(selectedDate, pg);

            ViewBag.Date = selectedDate;

            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await Visitservices.getvisitbyid(id);

            if (!result.Success)
            {
               
                return NotFound();
            }

            return View(result.Data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await Visitservices.getvisitbyid(id);

            if (!result.Success)
            {
                return NotFound();
            }

            var updateVm = new UpdateVisitVM
            {
                visitid = result.Data.visitid,
                notes = result.Data.notes,
                diagnosis = result.Data.diagnosis,
                visitstatus = result.Data.visitstatus
            };

            return View(updateVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateVisitVM vm)
        {
            if (ModelState.IsValid)
            {
                var result = await Visitservices.updatevisit(vm);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = result.Data.visitid });
                }

                ModelState.AddModelError("", result.Message);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await Visitservices.completevisit(id);

            if (result.Success)
            {
                TempData["Success"] = "Visit completed successfully. You can now add a prescription.";

                // Redirect to create a prescription for this visit
                return RedirectToAction("Create", "Prescription", new { visitId = id });
            }

            TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelVisit(int id)
        {
            var result = await Visitservices.cancelvisit(id);

            if (result.Success)
            {
                TempData["Success"] = "Visit canceled successfully and the appointment has been reverted to Confirmed status.";

                return RedirectToAction("Index", "Appoinment");
            }

            TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Visitservices.deletevisit(id);

            if (result.Success)
            {
                TempData["Success"] = "Visit deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}