using smart_clinic.Models;
using smart_clinic.viewmodels.prescriptionitems;
using System.ComponentModel.DataAnnotations.Schema;

namespace smart_clinic.viewmodels.prescription
{
    public class UpdatePrescriptionvm
    {
        public int prescriptionid { get; set; }
        public DateTime prescriptiondate { get; set; }
        public string notes { get; set; }
        public int? visitid { get; set; }

        public ICollection<UpdatePrescriptionitemvm> items { get; set; }=new List<UpdatePrescriptionitemvm>();
    }
}
