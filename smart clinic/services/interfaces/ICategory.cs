using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.Models;
using smart_clinic.viewmodels.category;
using smart_clinic.viewmodels.General;

namespace smart_clinic.services.interfaces
{
    public interface ICategory
    {
        public Task<ResponseStatus<ResponseCategoryVM>> createcategory(AddCategoryVM vm);
        //getallcat
        public Task<ResponseStatus<IEnumerable<ResponseCategoryVM>>> getallcategory(pagination pg, string searchterm, string? userId = null, bool? isActive = null);

        //getcatbyid
        public Task<ResponseStatus<ResponseCategoryVM>> getcategorybyid(int id);

        //updatecat
        public Task<ResponseStatus<ResponseCategoryVM>> updatecategory(UpdateCategoryVM vm);

        //removecat
        public Task<ResponseStatus<bool>> removecategory(int id);


        //getactivecat
        public  Task<ResponseStatus<IEnumerable<ResponseCategoryVM>>> getcategoryactive();
        
    }
}
