using AutoMapper;
using smart_clinic.Models;
using smart_clinic.viewmodels.medicine;

namespace smart_clinic.mapping
{
    public class medicinemapping:Profile
    {
        public medicinemapping() 
        {
            CreateMap<AddMedicineVM, Medicine>()
                  .ForMember(dest => dest.user_id, opt => opt.Ignore());
            CreateMap<Medicine, ResponseMedicineVM>();

        }
    }
}
