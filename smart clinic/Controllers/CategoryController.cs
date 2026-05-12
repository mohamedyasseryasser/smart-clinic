using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.category;
using smart_clinic.viewmodels.General;
using System.Collections;
using System.Security.Claims;
using System.Threading.Tasks;

namespace smart_clinic.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        public CategoryController(IUser userservice,ICategory categoryservice)
        {
            Userservice = userservice;
            Categoryservice = categoryservice;
        }
        public IUser Userservice { get; }
        public ICategory Categoryservice { get; }
        //loadadmins
        private async Task loadusers(UpdateCategoryVM vm)
        {
            var result =await Userservice.getallactiveadmins();
            vm.admins = result.Data.Select(d => new SelectListItem
            {
                Text = d.UserName,
                Value = d.Id.ToString()
            }).ToList();

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
            var user = await Userservice.getadminbyuserid(userid);
            if (!user.Success)
            {
                return false;
            }
            return true;
        }

        public async Task<IActionResult> Index(string searchterm = "", string? userId = null, bool? isActive = null, int pageNumber = 1, int pageSize = 10)
        {
            var pg = new pagination { PageNumber = pageNumber, PageSize = pageSize };
            var result = await Categoryservice.getallcategory(pg, searchterm, userId, isActive);

            // Load Admins for the dropdown
            var adminsResult = await Userservice.getallactiveadmins();
            ViewBag.Admins = adminsResult.Data.Select(d => new SelectListItem
            {
                Text = d.UserName,
                Value = d.Id.ToString(),
                Selected = d.Id.ToString() == userId
            }).ToList();

            ViewBag.SearchTerm = searchterm;
            ViewBag.SelectedUserId = userId;
            ViewBag.SelectedIsActive = isActive;

            return View(result.Data);
        }
        public async Task<IActionResult> createcategory()
        {
            var vm=new AddCategoryVM();
            var user = await getcurrentuser();
            if (!user.Success)
            {
                return View("Login");
            }
            if (!await isadmin(user.Data))
            {
                return View("Login");
            }
             return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> create(AddCategoryVM vm)
        {
            var user = await getcurrentuser();
            if (user.Success)
            {
                vm.user_id = user.Data;
            }
            if (!ModelState.IsValid)
            {
                return View("createcategory",vm);
            }
            var result=await Categoryservice.createcategory(vm);
            if (!result.Success)
            {
                ModelState.AddModelError("",result.Message);
                 return View("createcategory",vm);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            var result = await Categoryservice.getcategorybyid(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }
            return View(result.Data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await Categoryservice.getcategorybyid(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var vm = new UpdateCategoryVM
            {
                cat_id = result.Data.cat_id,
                cat_name = result.Data.cat_name,
                cat_description = result.Data.cat_description,
                isactive = result.Data.isactive,
                user_id=result.Data.user_id
            };
            await loadusers(vm);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryVM vm)
        {
            if (!ModelState.IsValid)
            {
                await loadusers(vm);
                return View(vm);
            }

            var result = await Categoryservice.updatecategory(vm);
            if (!result.Success)
            {
                await loadusers(vm);
                ModelState.AddModelError("", result.Message);
                return View(vm);
            }

            TempData["Success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Categoryservice.removecategory(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Category deleted successfully";
            }
            return RedirectToAction("Index");
        }

    }
}
