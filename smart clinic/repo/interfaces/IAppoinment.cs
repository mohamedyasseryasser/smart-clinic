using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.Models;
using smart_clinic.viewmodels.Appoinment;
using smart_clinic.viewmodels.General;

namespace smart_clinic.services.interfaces
{
    public interface IAppoinment
    {
        Task<ResponseStatus<ResponseAppoimentVM>> CreateAppoinment(AddAppoinmentVM vm);

        Task<ResponseStatus<ResponseAppoimentVM>> UpdateAppoinment(UpdateAppoinment vm);

        Task<ResponseStatus<ResponseAppoimentVM>> UpdateAppoinmentState(UpdateAppoinmentStateVM vm);

        Task<ResponseStatus<ResponseAppoimentVM>> GetAppoinmentById(int id);

        Task<ResponseStatus<IEnumerable<ResponseAppoimentVM>>> SearchAppoinment(SearchAppoinmentVM vm);

        Task<ResponseStatus<bool>> CancelAppoinment(int id);
        public  Task<ResponseStatus<IEnumerable<ResponseAppoimentVM>>> getallappoinment(DateTime? date, pagination pg)
        ;
    }
}
