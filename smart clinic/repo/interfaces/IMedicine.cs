using smart_clinic.Models;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.medicine;

namespace smart_clinic.services.interfaces
{
    public interface IMedicine
    {

        Task<ResponseStatus<IEnumerable<ResponseMedicineVM>>> GetAllAsync(pagination pg, string searchTerm = null, int? categoryId = null,bool? isdeleted=null);
        Task<ResponseStatus<ResponseMedicineVM>> GetByIdAsync(int id);
        Task<ResponseStatus<ResponseMedicineVM>> AddAsync(AddMedicineVM vm);
        Task<ResponseStatus<ResponseMedicineVM>> UpdateAsync(UpdateMedicineVM vm);
        Task<ResponseStatus<bool>> DeleteAsync(int id);
        Task<ResponseStatus<bool>> ExistsAsync(int id);
        public   Task<ResponseStatus<IEnumerable<ResponseMedicineVM>>> getactivemedicine();
        public  Task<decimal> GetPrescriptionCostAsync(int prescriptionId);

    }
}
