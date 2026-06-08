using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smart_clinic.filter;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.medicine;
using System.Security.Claims;
using System.Threading.Tasks;

namespace smart_clinic.Controllers
{
    [Authorize(Roles = "Admin")]
    // [NoCache]
    // [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class MedicineController : Controller
    {
        private readonly IMedicine _medicineRepo;
        private readonly ICategory _categoryRepo;
        private readonly IUser userservice;
        private readonly ILogger<MedicineController> logger;

        public MedicineController(IMedicine medicineRepo, ICategory categoryRepo, IUser userservice,ILogger<MedicineController> logger)
        {
            _medicineRepo = medicineRepo;
            _categoryRepo = categoryRepo;
            this.userservice = userservice;
            this.logger = logger;
        }
        //getcurrentuser
        private async Task<ResponseStatus<string>> getcurrentuser()
        {
            var respons = new ResponseStatus<string>();
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                respons.Success = false;
                respons.Message = "please login";
                return respons;
            }
            else
            {
                respons.Success = true;
                respons.Data = userid.ToString();
                return respons;
            }
        }
        //check is admin or not
        private async Task<bool> isadmin(string userid)
        {
            var user = await userservice.getadminbyuserid(userid);
            if (!user.Success)
            {
                return false;
            }
            return true;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, bool? isdeleted, int? cat_id, int pageNumber = 1, int pageSize = 10)
        {
            var pg = new pagination { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _medicineRepo.GetAllAsync(pg, searchTerm, cat_id, isdeleted);

            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = cat_id;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.isdaleted = isdeleted;
            // Load categories for the filter dropdown
            var categories = await _categoryRepo.getcategoryactive();
            ViewBag.Categories = new SelectList(categories.Data, "cat_id", "cat_name", cat_id);
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _medicineRepo.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await getcurrentuser();
            if (!user.Success)
            {
                return View("Login");
            }
            if (!await isadmin(user.Data))
            {
                return View("Login");
            }
            var vm = new AddMedicineVM();
            await LoadCategories(vm);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddMedicineVM vm)
        {
            var user = await getcurrentuser();
            if (user.Success)
            {
                vm.user_id = user.Data;
            }
            if (!ModelState.IsValid)
            {
                await LoadCategories(vm);
                return View(vm);
            }

            var result = await _medicineRepo.AddAsync(vm);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Medicine created successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            await LoadCategories(vm);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _medicineRepo.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            var user = await getcurrentuser();
            if (!user.Success)
            {
                return View("Login");
            }
            if (!await isadmin(user.Data))
            {
                return View("Login");
            }
            var updateVm = new UpdateMedicineVM
            {
                medicineId = result.Data.medicineId,
                Name = result.Data.Name,
                StockQuantity = result.Data.StockQuantity,
                SupplierName = result.Data.SupplierName,
                IsDeleted = result.Data.IsDeleted,  
                user_id=user.Data,
                UnitPrice = result.Data.UnitPrice,
                RecordLevel = result.Data.RecordLevel,
                ExpiryDate = result.Data.ExpiryDate,
                cat_id = result.Data.cat_id
            };

            await LoadCategories(updateVm);
            return View(updateVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMedicineVM vm)
        {
            if (!ModelState.IsValid)
            {
              await  LoadCategories(vm);
                return View(vm);
            }

            var result = await _medicineRepo.UpdateAsync(vm);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Medicine updated successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
        await    LoadCategories(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _medicineRepo.DeleteAsync(id);
            if (result.Success)
            {
                logger.LogInformation( "Medicine deleted successfully");
            }
            else
            {
                logger.LogWarning( result.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategories(AddMedicineVM vm)
        {
            var categories = await _categoryRepo.getcategoryactive();
            vm.Categories = categories.Data.Select(c => new SelectListItem
            {
                Value = c.cat_id.ToString(),
                Text = c.cat_name
            }).ToList();
        }
    }
    }