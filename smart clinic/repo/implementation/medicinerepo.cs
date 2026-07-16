using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.enums;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.medicine;
namespace smart_clinic.services.reporesity

{
    public class medicinerepo : IMedicine
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ICategory categoryservice;

        public medicinerepo(Context context, IMapper mapper, ICategory categoryservice)
        {
            _context = context;
            _mapper = mapper;
            this.categoryservice = categoryservice;
        }
        public async Task<ResponseStatus<IEnumerable<ResponseMedicineVM>>> GetAllAsync(pagination pg,
            string searchTerm = "", int? categoryId = null, bool? isdeleted = null)
        {
            var response = new ResponseStatus<IEnumerable<ResponseMedicineVM>>();
            try
            {
                IQueryable<Medicine> query = _context.Medicines.Include(m => m.category).AsNoTracking();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    string search = searchTerm.ToLower();
                    query = query.Where(m => m.Name.ToLower().Contains(search)
                    || m.SupplierName.ToLower().Contains(search));
                }
                if (categoryId.HasValue)
                {
                    query = query.Where(m => m.cat_id == categoryId.Value);
                }
                if (isdeleted.HasValue && isdeleted != null)
                {
                    query = query.Where(m => m.IsDeleted == isdeleted.Value);
                }
                var totalCount = await query.CountAsync();
                var items = await query.OrderByDescending(m => m.CreatedAt)
                                      .Skip((pg.PageNumber - 1) * pg.PageSize)
                                      .Take(pg.PageSize)
                                      .ToListAsync();

                response.Data = _mapper.Map<IEnumerable<ResponseMedicineVM>>(items);
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving medicines";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<ResponseMedicineVM>> GetByIdAsync(int id)
        {
            var response = new ResponseStatus<ResponseMedicineVM>();
            try
            {
                var medicine = await _context.Medicines.
                    Include(m => m.category).
                    FirstOrDefaultAsync(m => m.medicineId == id);
                if (medicine == null)
                {
                    response.Success = false;
                    response.Message = "Medicine not found";
                    return response;
                }

                response.Data = _mapper.Map<ResponseMedicineVM>(medicine);
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving medicine";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
        public async Task<ResponseStatus<ResponseMedicineVM>> AddAsync(AddMedicineVM vm)
        {
            var response = new ResponseStatus<ResponseMedicineVM>();
            try
            {
                //check category
                var category = await _context.categories.FirstOrDefaultAsync(c => c.cat_id == vm.cat_id && c.isactive != false);
                if (category == null)
                {
                    response.Success = false;
                    response.Message = "this category is not found";
                    return response;
                }
                var user = await _context.Admins.Include(c => c.user).
                    FirstOrDefaultAsync(u => u.userid == vm.user_id && u.status != userstatus.inactive);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "this user is not found";
                    return response;
                }
                var medicine = _mapper.Map<Medicine>(vm);
                medicine.CreatedAt = DateTime.Now;
                medicine.user_id = vm.user_id;
                medicine.IsDeleted = false;
                medicine.AddedBy = user.user.UserName;
                medicine.CategoryName = category.cat_name;
                await _context.Medicines.AddAsync(medicine);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<ResponseMedicineVM>(medicine);
                response.Success = true;
                response.Message = "Medicine added successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error adding medicine";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<ResponseMedicineVM>> UpdateAsync(UpdateMedicineVM vm)
        {
            var response = new ResponseStatus<ResponseMedicineVM>();
            try
            {
                var existing = await _context.Medicines.FindAsync(vm.medicineId);
                if (existing == null)
                {
                    response.Success = false;
                    response.Message = "Medicine not found";
                    return response;
                }
                existing.user_id = vm.user_id;
                _mapper.Map(vm, existing);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<ResponseMedicineVM>(existing);
                response.Success = true;
                response.Message = "Medicine updated successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating medicine";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<bool>> DeleteAsync(int id)
        {
            var response = new ResponseStatus<bool>();
            try
            {
                var medicine = await _context.Medicines.FindAsync(id);
                if (medicine == null)
                {
                    response.Success = false;
                    response.Message = "Medicine not found";
                    return response;
                }

                medicine.IsDeleted = true;
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Medicine deleted successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error deleting medicine";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<bool>> ExistsAsync(int id)
        {
            var response = new ResponseStatus<bool>();
            try
            {
                var exists = await _context.Medicines.AnyAsync(m => m.medicineId == id && m.IsDeleted == true);
                response.Data = exists;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error checking existence";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
        public async Task<ResponseStatus<IEnumerable<ResponseMedicineVM>>> getactivemedicine()
        {
            var response = new ResponseStatus<IEnumerable<ResponseMedicineVM>>();
            try
            {
                IQueryable<Medicine> query= _context.Medicines.
                    Include(m=>m.category)
                    .Where(m=>m.IsDeleted!=true&&m.category.isactive==true);
                if (query==null)
                {
                    response.Success=false;
                    response.Message = "not found medicine";
                    response.Errors.Add(response.Message);
                    return response;
                }
                var medicines =await  query.ToListAsync();
                response.Success = true;
                response.Data = _mapper.Map<IEnumerable<ResponseMedicineVM>>(medicines);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"{ex.Message}";
                response.Errors.Add(response.Message);
                return response;
            }
        }
    }
}
