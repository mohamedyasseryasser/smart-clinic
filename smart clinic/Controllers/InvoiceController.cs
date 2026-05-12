using Microsoft.AspNetCore.Mvc;

namespace smart_clinic.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
