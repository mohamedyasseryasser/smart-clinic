using smart_clinic.enums;
using smart_clinic.viewmodels.Patient;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.Appoinment
{
    public class ResponseAppoimentVM
    {
        public int appoimentid { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Appoinmentdate { get; set; }
        public DateTime startat { get; set; }
        public DateTime endat { get; set; }
        public DateTime updateat { get; set; }
        public string? notes { get; set; }
        public AppointmentStatus status { get; set; }
        public int doctorid { get; set; }
        public int resptionistidid {  get; set; }
        public decimal cost { get; set; }
        public typeofappoinment type { get; set; }
        public ResponsePatientVM? Patient { get; set; }
    }
}
