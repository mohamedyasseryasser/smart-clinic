using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.enums;
using smart_clinic.Models;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Visit;
using smart_clinic.viewmodels.VisitRepo;

namespace smart_clinic.services.interfaces
{
    public interface IVisit
    {
      
        public  Task<ResponseStatus<ResponseVisitVM>> createvisit(AddVisit vm);

         public Task<ResponseStatus<ResponseVisitVM>> getvisitbyid(int id);

         public Task<ResponseStatus<ResponseVisitVM>> updatevisit(UpdateVisitVM vm);

        public  Task<ResponseStatus<IEnumerable<ResponseVisitVM>>> getallvisit(DateTime date, pagination pg)
        ;

    }
}
