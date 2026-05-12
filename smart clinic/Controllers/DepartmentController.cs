using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_clinic.filter;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.departmentvm;
using smart_clinic.viewmodels.General;
using System.Security.Claims;
namespace smart_clinic.Controllers
{
    [Authorize]
    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class DepartmentController : Controller
    {
        private readonly IDepartment _departmentRepo;

        public DepartmentController(IDepartment departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string phone, bool? isactive, string name, int? departmentid, int pageNumber = 1, int pageSize = 10)
        {
            var pg = new pagination { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _departmentRepo.GetAllAsync(pg, phone, isactive, name, departmentid);

            ViewBag.Phone = phone;
            ViewBag.IsActive = isactive;
            ViewBag.Name = name;
            ViewBag.DepartmentId = departmentid;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _departmentRepo.GetByIdWithDoctorsAsync(id);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddDeptVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            vm.userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (vm.userid==null)
            {
                ModelState.AddModelError(string.Empty,"unauthorized");

                return View(vm);

            }
            var result = await _departmentRepo.AddAsync(vm);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Department created successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _departmentRepo.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var updateVm = new UpdateDertVm
            {
                DepartmentId = result.Data.DepartmentId,
                Name = result.Data.Name,
                FloorNumber = result.Data.FloorNumber,
                Phone = result.Data.Phone,
                description = result.Data.description,
                isactive = result.Data.isactive,
                userid = result.Data.userid
            };

            return View(updateVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDertVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _departmentRepo.UpdateAsync(vm);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Department updated successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentRepo.DeleteAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Department deleted (deactivated) successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}