using Microsoft.AspNetCore.Mvc;
using smart_clinic.repo.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Invoice;

namespace smart_clinic.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoice _invoiceService;

        public InvoiceController(IInvoice invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string patientName = null, DateTime? date = null)
        {
            var pagination = new pagination { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _invoiceService.GetAllInvoicesAsync(pagination, patientName, date);
            if (result.Success)
            {
                return View(result.Data);
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult Create(int? visitId)
        {
            var vm = new createinvoicevm();
            if (visitId.HasValue)
            {
                vm.VisitId = visitId.Value;
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> submit(createinvoicevm vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _invoiceService.CreateInvoiceAsync(vm);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _invoiceService.GetInvoiceByIdAsync(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _invoiceService.GetInvoiceByIdAsync(id);
            if (result.Success)
            {
                var updateVm = new updateinvoicevm
                {
                    InvoiceId = result.Data.InvoiceId,
                    Tax = result.Data.Tax,
                    Discount = result.Data.Discount,
                    Status = result.Data.Status
                };
                return View(updateVm);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(updateinvoicevm vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _invoiceService.UpdateInvoiceAsync(vm);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _invoiceService.CancelInvoiceAsync(id);
            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int id)
        {
            var result = await _invoiceService.FinishInvoiceAsync(id);
            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
