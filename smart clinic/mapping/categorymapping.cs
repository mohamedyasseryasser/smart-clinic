using AutoMapper;
using smart_clinic.Models;
using smart_clinic.viewmodels.category;

namespace smart_clinic.mapping
{
    public class categorymapping:Profile
    {
        public categorymapping()
        {
            CreateMap<Category, ResponseCategoryVM>()
                .ForMember(des=>des.AddedBy,
                opt=>opt.MapFrom(src=>src.user.UserName));
            CreateMap<AddCategoryVM, Category>();
            CreateMap<UpdateCategoryVM, Category>();
           // CreateMap<UpdateCategoryVM, ResponseCategoryVM>();
        }
    }
}
