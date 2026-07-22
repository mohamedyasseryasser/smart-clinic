using AutoMapper;
using smart_clinic.Models;
using smart_clinic.viewmodels.Invoice;

namespace smart_clinic.mapping
{
    public class invoicemapping : Profile
    {
        public invoicemapping()
        {
            CreateMap<Invoice, responseinvoicevm>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Visit.Appoinment.Patient.patientname))
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => src.Visit.visitdate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<createinvoicevm, Invoice>();
        }
    }
}
