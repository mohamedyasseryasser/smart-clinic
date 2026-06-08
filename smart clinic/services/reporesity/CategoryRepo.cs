using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.category;
using smart_clinic.viewmodels.General;
using System.Threading.Tasks;

namespace smart_clinic.services.reporesity
{
    public class CategoryRepo:ICategory
    {
        public CategoryRepo(Context context,IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public Context Context { get; }
        public IMapper Mapper { get; }
        //createcategory
        public async Task<ResponseStatus<ResponseCategoryVM>> createcategory(AddCategoryVM vm)
        {
            var response = new ResponseStatus<ResponseCategoryVM>();
            try
            {
                var category = Mapper.Map<Category>(vm);
                category.isactive = true;
                await   Context.categories.AddAsync(category);
                await  Context.SaveChangesAsync();
                var newcat=await Context.categories.
                    Include(c=>c.user).
                    FirstOrDefaultAsync(c=>c.cat_id==category.cat_id);
                var data = Mapper.Map<ResponseCategoryVM>(newcat);
                  response.Data = data;
                response.Success = true;
                response.Message = "Category created successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message =$"exit wrong {ex.Message}";
                return response;
            }
            
        }
        //getallcat
      public  async Task<ResponseStatus<IEnumerable<ResponseCategoryVM>>> getallcategory(pagination pg, string searchterm, string? userId = null, bool? isActive = null)
        {
            var response = new ResponseStatus<IEnumerable<ResponseCategoryVM>>();
            try
            {
                IQueryable<Category> query = Context.categories.Include(c => c.user).AsNoTracking().AsQueryable();

                // Filter by Search Term (Name or Description)
                if (!string.IsNullOrEmpty(searchterm))
                {
                    var lowerSearch = searchterm.ToLower();
                    query = query.Where(c => c.cat_name.ToLower().Contains(lowerSearch) || c.cat_description.ToLower().Contains(lowerSearch));
                }

                // Filter by Admin (User Id)
                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(c => c.user_id == userId);
                }

                // Filter by IsActive status
                if (isActive.HasValue)
                {
                    query = query.Where(c => c.isactive == isActive.Value);
                }

                var categories =await query.Skip((pg.PageNumber - 1) * pg.PageSize).Take(pg.PageSize).ToListAsync();
                response.Data = Mapper.Map<IEnumerable<ResponseCategoryVM>>(categories);
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"exit wrong {ex.Message}";
                return response;

            }
      }  
        //getcatbyid
        public async Task<ResponseStatus<ResponseCategoryVM>> getcategorybyid(int id)
        {
            var response = new ResponseStatus<ResponseCategoryVM>();
            try
            {
                var category =await Context.categories.
                    AsNoTracking().
                    FirstOrDefaultAsync(c => c.cat_id == id);
                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category not found";

                }
                else
                {
                    response.Data = Mapper.Map<ResponseCategoryVM>(category);
                    response.Data.AddedBy = category.AddedBy;
                    response.Success = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"exit wrong {ex.Message}"; 
                return response;
            }
        }
        //updatecat
        public async Task<ResponseStatus<ResponseCategoryVM>> updatecategory(UpdateCategoryVM vm)
        {
            var response = new ResponseStatus<ResponseCategoryVM>();
            try
            {
                var category = await Context.categories.
                    Include(c => c.user)
                    .FirstOrDefaultAsync(c => c.cat_id == vm.cat_id);
                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category not found";
                }
                else
                {
                    var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == vm.user_id);
                    if (user == null)
                    {
                        response.Success = false;
                        response.Message = $"user is not found";
                        return response;
                    }

                    category.cat_name = vm.cat_name;
                    category.cat_description = vm.cat_description;
                    category.isactive = vm.isactive;
                    category.user_id = vm.user_id;
                    category.AddedBy = user.UserName;

                    await Context.SaveChangesAsync();

                    response.Data = Mapper.Map<ResponseCategoryVM>(category);
                    response.Success = true;
                    response.Message = "Category updated successfully";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"exit wrong {ex.Message}";
                return response;
            }
        }
        //removecat
        public async Task<ResponseStatus<bool>> removecategory(int id)
        {
            var response = new ResponseStatus<bool>();
            try
            {
                var category =await Context.categories.FindAsync(id);
                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category not found";
                }
                else
                {
                    category.isactive = false;
                  await  Context.SaveChangesAsync();
                    response.Data = true;
                    response.Success = true;
                    response.Message = "Category deleted successfully";
                }
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = false; return response;

            }
        }

        //getactivecat
        public async Task<ResponseStatus<IEnumerable<ResponseCategoryVM>>> getcategoryactive()
        {
            var response = new ResponseStatus<IEnumerable<ResponseCategoryVM>>();
            try
            {
                var categories =await Context.categories.
                    Where(c=>c.isactive==true).
                    AsNoTracking().
                    ToListAsync();
                response.Data = Mapper.Map<IEnumerable<ResponseCategoryVM>>(categories);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

       
      
    }
}
