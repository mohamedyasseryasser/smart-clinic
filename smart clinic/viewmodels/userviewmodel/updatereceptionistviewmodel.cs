using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.userviewmodel
{
    public class updatereceptionistviewmodel:updateuserviewmodel
    {
        [Required]
        public int receptionid {  get; set; }
        [Required]
        public decimal salary { get; set; }
        public DateTime hiredate { get; set; } = DateTime.Now;
    }
}
