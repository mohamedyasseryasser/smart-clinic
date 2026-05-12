using AutoMapper;
using smart_clinic.Models;
using smart_clinic.viewmodels.departmentvm;
using smart_clinic.viewmodels.Visit;
using smart_clinic.viewmodels.VisitRepo;

namespace smart_clinic.mapping
{
    public class visitmapping:Profile
    {
        public visitmapping() 
        {
            CreateMap<AddVisit,Visit>(); 
            CreateMap<Visit, ResponseVisitVM>();
            CreateMap<UpdateVisitVM, ResponseVisitVM>();
        }
    }
}
