using Microsoft.AspNetCore.Mvc;

namespace smart_clinic.Controllers
{
    public class PrescriptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
