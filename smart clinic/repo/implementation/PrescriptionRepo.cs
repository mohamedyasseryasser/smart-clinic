using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.prescription;
using smart_clinic.viewmodels.prescriptionitems;

namespace smart_clinic.services.reporesity
{
    public class PrescriptionRepo:IPrescription
    {
        public Context _context { get; }
        public IMapper _mapper { get; }
        public PrescriptionRepo(Context context,IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
      
        public async Task<ResponseStatus<ResponsePrescriptionVM>> CreatePrescription(AddPrescriptionVM vm)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var response = new ResponseStatus<ResponsePrescriptionVM>();
            try
            {
                Prescription prescription = _mapper.Map<Prescription>(vm);
                prescription.prescriptiondate = DateTime.Now;

                if (vm.visitid.HasValue)
                {
                    var exitvisit = await _context.Visits.FindAsync(vm.visitid.Value);
                    if (exitvisit == null)
                    {
                        response.Success = false;
                        response.Message = "this visit id is not found";
                        response.Errors.Add(response.Message);
                        return response;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "visit id is required";
                    response.Errors.Add(response.Message);
                    return response;
                }

                await _context.Prescriptions.AddAsync(prescription);
                await _context.SaveChangesAsync();

                foreach (var itemVm in vm.items)
                {
                    Prescriptionitems newitem = _mapper.Map<Prescriptionitems>(itemVm);
                    newitem.prescriptionid = prescription.prescriptionid;
                    
                    
                    var existingMedicine = await _context.Medicines.FindAsync(newitem.mdeicineid);
                    if (existingMedicine == null)
                    {
                        response.Success = false;
                        response.Message = $"Medicine with ID {newitem.mdeicineid} not found.";
                        response.Errors.Add(response.Message);
                        await transaction.RollbackAsync();
                        return response;
                    }
 
                    prescription.items.Add(newitem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Success = true;
                response.Message = "Prescription created successfully.";
                var data=await _context.Prescriptions.
                    Include(p=>p.Visit).
                    ThenInclude(v=>v.Appoinment).
                    ThenInclude(a=>a.Patient).Include(p=>p.items).ThenInclude(i=>i.Medicine).
                    FirstOrDefaultAsync(p=>p.prescriptionid==prescription.prescriptionid);
                response.Data = _mapper.Map<ResponsePrescriptionVM>(data);
           
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.Success = false;
                response.Message = "Error creating prescription: " + ex.Message;
                response.Errors.Add(ex.Message);
            }
            return response;
        }
        public async Task<ResponseStatus<IEnumerable<ResponsePrescriptionVM>>> GetAllPrescriptions(
     pagination pg,
     DateTime? date = null)
        {
            var response = new ResponseStatus<IEnumerable<ResponsePrescriptionVM>>();

            try
            {
                var query = _context.Prescriptions
                    .Where(p => date == null || p.prescriptiondate.Date == date.Value.Date)
                    .Include(p => p.items).ThenInclude(i=>i.Medicine)
                    .Include(p => p.Visit)
                        .ThenInclude(v => v.Appoinment)
                            .ThenInclude(a => a.Patient)
                    .OrderByDescending(p => p.prescriptiondate)
                    .AsQueryable();

                var prescriptions = await query
                    .Skip((pg.PageNumber - 1) * pg.PageSize)
                    .Take(pg.PageSize)
                    .ToListAsync();

                response.Success = true;
                response.Data = _mapper.Map<IEnumerable<ResponsePrescriptionVM>>(prescriptions);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving prescriptions: " + ex.Message;
                response.Errors.Add(ex.Message);
                return response;
            }
        }
        public async Task<ResponseStatus<ResponsePrescriptionVM>> GetPrescriptionById(int id)
        {
            var response = new ResponseStatus<ResponsePrescriptionVM>();
            try
            {
                var prescription = await _context.Prescriptions.
                    Include(p=>p.Visit).
                    ThenInclude(v=>v.Appoinment)
                    .ThenInclude(a=>a.Patient)
                                            .Include(p => p.items)
                                            .ThenInclude(pi => pi.Medicine)
                                            .FirstOrDefaultAsync(p => p.prescriptionid == id);
                if (prescription == null)
                {
                    response.Success = false;
                    response.Message = "Prescription not found.";
                    response.Errors.Add(response.Message);
                    return response;
                }
                response.Success = true;
                response.Message = "Prescription retrieved successfully.";
                response.Data = _mapper.Map<ResponsePrescriptionVM>(prescription);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving prescription: " + ex.Message;
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<ResponsePrescriptionVM>> UpdatePrescription(UpdatePrescriptionvm vm)
        {
            var response = new ResponseStatus<ResponsePrescriptionVM>();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingPrescription = await _context.Prescriptions.Include(p => p.Visit).
                    ThenInclude(v => v.Appoinment)
                    .ThenInclude(a => a.Patient)
                                                    .Include(p => p.items).ThenInclude(i => i.Medicine)
                                                    .FirstOrDefaultAsync(p => p.prescriptionid == vm.prescriptionid);
                if (existingPrescription == null)
                {
                    response.Success = false;
                    response.Message = "Prescription not found.";
                    response.Errors.Add(response.Message);
                    return response;
                }
                //update basic prescription info
                existingPrescription.notes = vm.notes;
                existingPrescription.prescriptiondate = vm.prescriptiondate;
                //get olditems id
                var olditemids = existingPrescription.items.Select(i => i.prescriptionitemid).ToList();
                //get newitems id
                var newitemids = vm.items.Select(i => i.prescriptionitemid).ToList();
                //get items in oldlistitems and removed from newlistitems
                var removeditems = existingPrescription.items.
                    Where(i => !newitemids.Contains(i.prescriptionitemid));
                //removed
                _context.PrescriptionItems.RemoveRange(removeditems);
                //add or update
                foreach (var item in vm.items)
                {
                    //add
                    if (item.prescriptionitemid == null || item.prescriptionitemid == 0)
                    {
                        var newItem = _mapper.Map<Prescriptionitems>(item);
                        newItem.prescriptionid = existingPrescription.prescriptionid;
                        existingPrescription.items.Add(newItem);
                    }
                    //update
                    else if (olditemids.Contains(item.prescriptionitemid.Value) && item.prescriptionitemid.HasValue)
                    {
                        var exititem = existingPrescription.items.
                            FirstOrDefault(i => i.prescriptionitemid == item.prescriptionitemid.Value);
                        _mapper.Map(item, exititem);
                        _context.PrescriptionItems.Update(exititem);
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Success = true;
                response.Message = "Prescription updated successfully.";
                response.Data = _mapper.Map<ResponsePrescriptionVM>(existingPrescription);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.Success = false;
                response.Message = "Error updating prescription: " + ex.Message;
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<ResponseStatus<string>> DeletePrescription(int id)
        {
            var response = new ResponseStatus<string>();
            try
            {
                var prescription = await _context.Prescriptions.
                    Include(p=>p.items).
                    FirstOrDefaultAsync(i=>i.prescriptionid==id);
                if (prescription == null)
                {
                    response.Success = false;
                    response.Message = "Prescription not found.";
                    response.Errors.Add(response.Message);
                    return response;
                }
                _context.PrescriptionItems.RemoveRange(prescription.items);
                _context.Prescriptions.Remove(prescription);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Prescription deleted successfully."; return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error deleting prescription: " + ex.Message;
                response.Errors.Add(ex.Message); return response;

            }
        }

        public async Task<ResponseStatus<ResponseRescriptionitemVM>> AddPrescriptionItem(AddPrescriptionItemVM vm)
        {
            var response = new ResponseStatus<ResponseRescriptionitemVM>();
            try
            {
                var prescription = await _context.Prescriptions.FindAsync(vm.prescriptionid);
                if (prescription == null)
                {
                    response.Success = false;
                    response.Message = "Prescription not found.";
                    response.Errors.Add(response.Message);
                    return response;
                }

                var medicine = await _context.Medicines.FindAsync(vm.mdeicineid);
                if (medicine == null)
                {
                    response.Success = false;
                    response.Message = "Medicine not found.";
                    response.Errors.Add(response.Message);
                    return response;
                }

                Prescriptionitems newItem = _mapper.Map<Prescriptionitems>(vm);
                
                prescription.items.Add(newItem);
                await _context.SaveChangesAsync();
                var item=await _context.PrescriptionItems.Include(i=>i.Medicine).
                    FirstOrDefaultAsync(p=>p.prescriptionitemid == newItem.prescriptionitemid);
                response.Success = true;
                response.Message = "Prescription item added successfully.";
                response.Data = _mapper.Map<ResponseRescriptionitemVM>(newItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error adding prescription item: " + ex.Message;
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<ResponseStatus<ResponseRescriptionitemVM>> UpdatePrescriptionItem(UpdatePrescriptionitemvm vm)
        {
            var response = new ResponseStatus<ResponseRescriptionitemVM>();
            try
            {
                var existingItem = await _context.PrescriptionItems.
                    Include(i=>i.Medicine).
                    FirstOrDefaultAsync(i=>i.prescriptionitemid==vm.prescriptionitemid);
                if (existingItem == null)
                {
                    response.Success = false;
                    response.Message = "Prescription item not found.";
                    response.Errors.Add(response.Message);
                    return response;
                }

                _mapper.Map(vm, existingItem);
                _context.PrescriptionItems.Update(existingItem);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Prescription item updated successfully.";
                response.Data = _mapper.Map<ResponseRescriptionitemVM>(existingItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating prescription item: " + ex.Message;
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<ResponseStatus<string>> DeletePrescriptionItem(int id)
        {
            var response = new ResponseStatus<string>();
            try
            {
                var item = await _context.PrescriptionItems.FindAsync(id);
                if (item == null)
                {
                    response.Success = false;
                    response.Message = "Prescription item not found.";
                    response.Errors.Add(response.Message);
                    return response;
                }
                _context.PrescriptionItems.Remove(item);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Prescription item deleted successfully."; 
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error deleting prescription item: " + ex.Message;
                response.Errors.Add(ex.Message);
                return response;

            }
        }
    }
}
