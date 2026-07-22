using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Invoice;

namespace smart_clinic.repo.interfaces
{
    public interface IInvoice
    {
        Task<ResponseStatus<responseinvoicevm>> CreateInvoiceAsync(createinvoicevm vm);
        Task<ResponseStatus<responseinvoicevm>> GetInvoiceByIdAsync(int id);
        Task<ResponseStatus<IEnumerable<responseinvoicevm>>> GetAllInvoicesAsync(pagination pg, string patientName = null, DateTime? date = null);
        Task<ResponseStatus<responseinvoicevm>> UpdateInvoiceAsync(updateinvoicevm vm);
        Task<ResponseStatus<bool>> CancelInvoiceAsync(int id);
        Task<ResponseStatus<bool>> FinishInvoiceAsync(int id);
        public Task<decimal> GetTotalRevenueAsync();

    }
}
