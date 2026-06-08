using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.prescription;
using smart_clinic.viewmodels.prescriptionitems;

namespace smart_clinic.services.interfaces
{
    public interface IPrescription
    {
        Task<ResponseStatus<ResponsePrescriptionVM>> CreatePrescription(AddPrescriptionVM vm);
        Task<ResponseStatus<IEnumerable<ResponsePrescriptionVM>>> GetAllPrescriptions(pagination pg,DateTime? date);
        Task<ResponseStatus<ResponsePrescriptionVM>> GetPrescriptionById(int id);
        Task<ResponseStatus<ResponsePrescriptionVM>> UpdatePrescription(UpdatePrescriptionvm vm);
        Task<ResponseStatus<string>> DeletePrescription(int id);
        Task<ResponseStatus<ResponseRescriptionitemVM>> AddPrescriptionItem(AddPrescriptionItemVM vm);
        Task<ResponseStatus<ResponseRescriptionitemVM>> UpdatePrescriptionItem(UpdatePrescriptionitemvm vm);
        Task<ResponseStatus<string>> DeletePrescriptionItem(int id);
    }
}
