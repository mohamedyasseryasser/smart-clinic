using AutoMapper;
using smart_clinic.Models;
using smart_clinic.viewmodels.departmentvm;

namespace smart_clinic.mapping
{
    public class deptmapping:Profile
    {
        public deptmapping() 
        {
            CreateMap<Department, ResponseDeptVm>().
                ForMember(dest => dest.UserName,
                          opt => opt.MapFrom(src => src.user.UserName))
               .ForMember(dest => dest.DoctorCount,
                          opt=>opt.MapFrom(src=>src.doctors.Count));

            CreateMap<AddDeptVm, Department>()
               .ForMember(dest => dest.createdat,
                          opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.updatedat,
                          opt => opt.Ignore()) 
               .ForMember(dest => dest.DepartmentId,
                          opt => opt.Ignore()); 
        }
    }
}
