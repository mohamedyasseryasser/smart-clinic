using Microsoft.AspNetCore.Mvc.Rendering;
using smart_clinic.viewmodels.prescriptionitems;

namespace smart_clinic.viewmodels.prescription
{
    public class AddPrescriptionVM
    {
        public DateTime prescriptiondate { get; set; }= DateTime.Now;
        public string? notes { get; set; }
        public ICollection<AddPrescriptionItemVM> items= new List<AddPrescriptionItemVM>();
        public int? visitid { get; set; }
        public int doctorid { get; set; }
        public int patientid { get; set; }

    }
}
