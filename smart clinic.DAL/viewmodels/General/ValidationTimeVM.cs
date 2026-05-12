using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.General
{
    public class ValidationTimeVM
    {
        public DateTime Appoinmentdate { get; set; }
        public DateTime startat { get; set; }
        public DateTime endat { get; set; }
    }
}
