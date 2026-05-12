using System.ComponentModel.DataAnnotations;

namespace smart_clinic.Models
{
    public class Medicine
    {
        [Key]
        public int medicineId { get; set; }

        public int StockQuantity { get; set; }

        public string Name { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Category { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int RecordLevel { get; set; }

        public DateTime ExpiryDate { get; set; }
        //navigation proberity
        public ICollection<Prescriptionitems> Prescriptionitems { get; set; } = new List<Prescriptionitems>();
    }
}
