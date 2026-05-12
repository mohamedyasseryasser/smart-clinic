using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.medicine
{
    public class AddMedicineVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Supplier name is required")]
        [MaxLength(100)]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [MaxLength(50)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Record level is required")]
        [Range(0, int.MaxValue)]
        public int RecordLevel { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int cat_id { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }
    }

    public class UpdateMedicineVM : AddMedicineVM
    {
        [Required]
        public int medicineId { get; set; }
    }

    public class ResponseMedicineVM
    {
        public int medicineId { get; set; }
        public int StockQuantity { get; set; }
        public string Name { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int RecordLevel { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int cat_id { get; set; }
        public string CategoryName { get; set; }
    }
}

