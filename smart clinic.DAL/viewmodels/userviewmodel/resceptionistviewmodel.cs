using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.userviewmodel
{
    public class resceptionistviewmodel:userviewmodel
    {
        [Required]
        public decimal salary { get; set; }
        public DateTime hiredate { get; set; }= DateTime.Now;
     }
}
