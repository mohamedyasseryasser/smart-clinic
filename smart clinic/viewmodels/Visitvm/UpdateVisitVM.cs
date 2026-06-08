using smart_clinic.enums;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.VisitRepo
{
    public class UpdateVisitVM
    {
        [Required(ErrorMessage = "visit id is required")]
        public int visitid { get; set; }
        public string? notes { get; set; }

        public string? diagnosis { get; set; }

        [Required(ErrorMessage = "statues is required.")]
        public VisitStatus visitstatus { get; set; }
    }
}
