using Microsoft.AspNetCore.Mvc.Rendering;
using smart_clinic.enums;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.userviewmodel
{
    public class doctorviewmodel:userviewmodel
    {
        [Required]
        [MinLength(4)]
        public string Specialization { get; set; }
         public DateTime hiredate { get; set; }= DateTime.UtcNow;
        [Required]
        public decimal salary { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public ICollection<SelectListItem> deptids { get; set; }= new List<SelectListItem>();
    }
}
