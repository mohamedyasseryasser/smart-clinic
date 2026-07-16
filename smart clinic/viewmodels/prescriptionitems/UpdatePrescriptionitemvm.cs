using Microsoft.AspNetCore.Mvc.Rendering;
using smart_clinic.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace smart_clinic.viewmodels.prescriptionitems
{
    public class UpdatePrescriptionitemvm
    {
        public int? prescriptionitemid { get; set; }
        public int quantity { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string notes { get; set; }
        public int mdeicineid { get; set; }
        public int? prescriptionid { get; set; }
        public ICollection<SelectListItem> medicines { get; set; } = new List<SelectListItem>();
    }
}
