using Microsoft.AspNetCore.Mvc;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Visit;
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
        public async Task<IActionResult> getallvisits(DateTime? date, int pageNumber = 1, int pageSize = 10)
        {
            if (date==null)
            {
                date = DateTime.Today;
            }
            var pg = new pagination { PageNumber = pageNumber, PageSize = pageSize };
            var result = await Visitservices.getallvisit(date.Value,pg);
            ViewBag.Date = date;    
            return View(result);
        }
    }
}
