using smart_clinic.enums;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.Visit
{
    public class AddVisit
    {
        public string? notes { get; set; }
         public string? diagnosis { get; set; }
        [Required(ErrorMessage ="appoinmentid is required")]
         public int appoinmentid { get; set; }
    }
}
