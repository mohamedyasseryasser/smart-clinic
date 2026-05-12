using smart_clinic.enums;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.userviewmodel
{
    public class responsereceptionistviewmodel:responseuserviewmodel
    {
        [Required]
        public int resptionistid { get; set; }
        public decimal salary { get; set; }
        public userstatus status { get; set; }
        public DateTime hiredate { get; set; }
    }
}
