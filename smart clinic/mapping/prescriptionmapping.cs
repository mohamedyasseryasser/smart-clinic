using AutoMapper;
using smart_clinic.Models;
using smart_clinic.viewmodels.prescription;
using smart_clinic.viewmodels.prescriptionitems;

namespace smart_clinic.mapping
{
    public class prescriptionmapping:Profile
    {
        public prescriptionmapping()
        {
            CreateMap<Prescription, ResponsePrescriptionVM>()
                .ForMember(dest => dest.patientid,
                    opt => opt.MapFrom(src =>
                        src.Visit.Appoinment.patientid))

                .ForMember(dest => dest.doctorid,
                    opt => opt.MapFrom(src =>
                        src.Visit.Appoinment.doctorid))

                .ForMember(dest => dest.rescriptionitems,
                    opt => opt.MapFrom(src => src.items))

              .ForMember(dest => dest.phonenumber,
                    opt => opt.MapFrom(src =>
                        src.Visit.Appoinment.PhoneNumber))

                .ForMember(dest => dest.patientname,
                    opt => opt.MapFrom(src =>
                        src.Visit.Appoinment.Patient.patientname)).ReverseMap();
            CreateMap<Prescription, AddPrescriptionVM>().ReverseMap();
            CreateMap<Prescription, UpdatePrescriptionvm>().ReverseMap();

            CreateMap<Prescriptionitems, ResponseRescriptionitemVM>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src =>
                        src.Medicine.Name)).ReverseMap();
            CreateMap<Prescription, AddPrescriptionVM>().ReverseMap(); 
            CreateMap<Prescriptionitems, AddPrescriptionItemVM>().ReverseMap();
            CreateMap<Prescriptionitems, UpdatePrescriptionitemvm>().ReverseMap();
        }
    }
}
