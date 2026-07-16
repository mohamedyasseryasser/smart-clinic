using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Patient;

namespace smart_clinic.services.interfaces
{
    public interface IPatient
    {
        Task<ResponseStatus<ResponsePatientVM>> CreatePatient(AddPatientVM vm);
        Task<ResponseStatus<ResponsePatientVM>> UpdatePatient(UpdatePatientVM vm);
        Task<ResponseStatus<bool>> RemovePatient(int id);
        Task<ResponseStatus<ResponsePatientVM>> getpatientbyid(int id);
        Task<ResponseStatus<ResponsePatientVM>> getpatientbyphonenumber(string phone);
        Task<ResponseStatus<ResponsePatientVM>> getpatientbyname(string name);
        public Task<ResponseStatus<IEnumerable<ResponsePatientVM>>> GetAllAsync(pagination pg,
                  string? patientname = null, string? phone = null, long? nationalid = null, bool? isactive = null);
        public Task<ResponseStatus<ResponsePatientVM>> GetPatientDetailsAsync(int id);
        public Task<ResponseStatus<int>> getpatientcount();

    }
}
