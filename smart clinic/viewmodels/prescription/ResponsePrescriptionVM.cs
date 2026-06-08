using smart_clinic.Models;
using smart_clinic.viewmodels.prescriptionitems;
using System.ComponentModel.DataAnnotations.Schema;

namespace smart_clinic.viewmodels.prescription
{
    public class ResponsePrescriptionVM
    {
        public int prescriptionid { get; set; }
        public DateTime prescriptiondate { get; set; }
        public string notes { get; set; }
        public int visitid { get; set; }
        public int doctorid { get; set; }
        public int patientid { get; set; }
        public string patientname {  get; set; }
        public string phonenumber {  get; set; }
        public ICollection<ResponseRescriptionitemVM> rescriptionitems { get; set; }=new List<ResponseRescriptionitemVM>();
    }
}
