using smart_clinic.Models;
using smart_clinic.viewmodels.departmentvm;
using smart_clinic.viewmodels.General;

namespace smart_clinic.services.interfaces
{
    public interface IDepartment
    {
        public Task<List<Department>> getalldept();
        public Task<ResponseStatus<IEnumerable<ResponseDeptVm>>> GetAllAsync(pagination pagination, string phone = null, bool? isactive = null, string name = null, int? departmentid = null);
        public Task<ResponseStatus<IEnumerable<ResponseDeptVm>>> GetActiveDepartmentsAsync();
        public Task<ResponseStatus<ResponseDeptVm>> GetByIdAsync(int id);
        public Task<ResponseStatus<ResponseDeptVm>> GetByIdWithDoctorsAsync(int id);
        public Task<ResponseStatus<ResponseDeptVm>> AddAsync(AddDeptVm department);
        public Task<ResponseStatus<ResponseDeptVm>> UpdateAsync(UpdateDertVm vm);
        public Task<ResponseStatus<bool>> DeleteAsync(int id);
        public Task<ResponseStatus<bool>> ExistsAsync(int id);
        public Task<IEnumerable<Department>> SearchAsync(string term);
        public Task<int> GetCountAsync();
    }
}
