namespace smart_clinic.viewmodels.Invoice
{
    public class responseinvoicevm
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal appoinmentcost {  get; set; }
        public decimal prescriptioncost {  get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int VisitId { get; set; }
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
