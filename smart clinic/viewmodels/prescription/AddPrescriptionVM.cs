using Microsoft.AspNetCore.Mvc.Rendering;
using smart_clinic.viewmodels.prescriptionitems;

namespace smart_clinic.viewmodels.prescription
{
    public class AddPrescriptionVM
    {
        public DateTime prescriptiondate { get; set; }= DateTime.Now;
        public string? notes { get; set; }
        public ICollection<AddPrescriptionItemVM> items { get; set; } = new List<AddPrescriptionItemVM>();
        public int? visitid { get; set; }
      

    }
}
