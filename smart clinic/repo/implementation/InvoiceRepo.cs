using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.enums;
using smart_clinic.Models;
using smart_clinic.repo.interfaces;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Invoice;
using System.Threading.Tasks;

namespace smart_clinic.repo.implementation
{
    public class InvoiceRepo : IInvoice
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IMedicine _medicineRepo;

        public InvoiceRepo(Context context, IMapper mapper, IMedicine medicineRepo)
        {
            _context = context;
            _mapper = mapper;
            _medicineRepo = medicineRepo;
        }

        public async Task<ResponseStatus<responseinvoicevm>> CreateInvoiceAsync(createinvoicevm vm)
        {
            var response = new ResponseStatus<responseinvoicevm>();
            try
            {
                var visit = await _context.Visits
                    .Include(v => v.Appoinment)
                    .Include(v => v.Prescription)
                    .FirstOrDefaultAsync(v => v.visitid == vm.VisitId);

                if (visit == null)
                {
                    response.Success = false;
                    response.Message = "Visit not found";
                    return response;
                }

                if (visit.visitstatus != VisitStatus.Completed)
                {
                    response.Success = false;
                    response.Message = "Visit must be completed before creating an invoice";
                    return response;
                }

                // Calculate costs
                decimal appointmentCost = visit.Appoinment?.cost ?? 0;
                decimal prescriptionCost = visit.Prescription != null
                    ? await _medicineRepo.GetPrescriptionCostAsync(visit.Prescription.prescriptionid)
                    : 0;

                decimal totalAmount = vm.cost + appointmentCost + prescriptionCost;
                decimal finalAmount = totalAmount + vm.Tax - vm.Discount;

                var invoice = new Invoice
                {
                    VisitId = vm.VisitId,
                    TotalAmount = totalAmount,
                    Tax = vm.Tax,
                    Discount = vm.Discount,
                    FinalAmount = finalAmount,
                    CreatedAt = DateTime.Now,
                    Status = InvoiceStatus.Pending
                };

                await _context.Invoices.AddAsync(invoice);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<responseinvoicevm>(invoice);
                response.Data.prescriptioncost = prescriptionCost;
                response.Data.appoinmentcost = appointmentCost;
                response.Success = true;
                response.Message = "Invoice created successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error creating invoice";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<responseinvoicevm>> GetInvoiceByIdAsync(int id)
        {
            var response = new ResponseStatus<responseinvoicevm>();
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Visit)
                        .ThenInclude(v => v.Appoinment)
                            .ThenInclude(a => a.Patient)
                    .FirstOrDefaultAsync(i => i.InvoiceId == id);

                if (invoice == null)
                {
                    response.Success = false;
                    response.Message = "Invoice not found";
                    return response;
                }
                // Calculate costs
                decimal appointmentCost = invoice.Visit?.Appoinment?.cost ?? 0;
                decimal prescriptionCost = invoice.Visit?.Prescription != null
                    ? await _medicineRepo.GetPrescriptionCostAsync(invoice.Visit.Prescription.prescriptionid)
                    : 0;

                response.Data = _mapper.Map<responseinvoicevm>(invoice);
                response.Data.prescriptioncost = prescriptionCost;
                response.Data.appoinmentcost = appointmentCost;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving invoice";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<IEnumerable<responseinvoicevm>>> GetAllInvoicesAsync(pagination pg, string patientName = null, DateTime? date = null)
        {
            var response = new ResponseStatus<IEnumerable<responseinvoicevm>>();
            try
            {
                var query = _context.Invoices
                    .Include(i => i.Visit)
                        .ThenInclude(v => v.Appoinment)
                            .ThenInclude(a => a.Patient)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(patientName))
                {
                    query = query.Where(i => i.Visit.Appoinment.Patient.patientname.Contains(patientName));
                }

                if (date.HasValue)
                {
                    query = query.Where(i => i.CreatedAt.Date == date.Value.Date);
                }

                var items = await query.OrderByDescending(i => i.CreatedAt)
                    .Skip((pg.PageNumber - 1) * pg.PageSize)
                    .Take(pg.PageSize)
                    .ToListAsync();

                var mappedItems = new List<responseinvoicevm>();
                foreach (var item in items)
                {
                    var mapped = _mapper.Map<responseinvoicevm>(item);
                    mapped.appoinmentcost = item.Visit?.Appoinment?.cost ?? 0;
                    mapped.prescriptioncost = item.Visit?.Prescription != null
                        ? await _medicineRepo.GetPrescriptionCostAsync(item.Visit.Prescription.prescriptionid)
                        : 0;
                    mappedItems.Add(mapped);
                }

                response.Data = mappedItems;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving invoices";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<responseinvoicevm>> UpdateInvoiceAsync(updateinvoicevm vm)
        {
            var response = new ResponseStatus<responseinvoicevm>();
            try
            {
                var invoice = await _context.Invoices.FindAsync(vm.InvoiceId);
                if (invoice == null)
                {
                    response.Success = false;
                    response.Message = "Invoice not found";
                    return response;
                }

                invoice.Tax = vm.Tax;
                invoice.Discount = vm.Discount;
                invoice.FinalAmount = invoice.TotalAmount + vm.Tax - vm.Discount;

                if (Enum.TryParse<InvoiceStatus>(vm.Status, out var status))
                {
                    invoice.Status = status;
                }

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<responseinvoicevm>(invoice);
                response.Success = true;
                response.Message = "Invoice updated successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating invoice";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<bool>> CancelInvoiceAsync(int id)
        {
            var response = new ResponseStatus<bool>();
            try
            {
                var invoice = await _context.Invoices.FindAsync(id);
                if (invoice == null)
                {
                    response.Success = false;
                    response.Message = "Invoice not found";
                    return response;
                }

                invoice.Status = InvoiceStatus.Cancelled;
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Invoice cancelled successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error cancelling invoice";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<ResponseStatus<bool>> FinishInvoiceAsync(int id)
        {
            var response = new ResponseStatus<bool>();
            try
            {
                var invoice = await _context.Invoices.FindAsync(id);
                if (invoice == null)
                {
                    response.Success = false;
                    response.Message = "Invoice not found";
                    return response;
                }

                invoice.Status = InvoiceStatus.Paid;
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Invoice marked as paid";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error finishing invoice";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Invoices
                .Where(i => i.Status == InvoiceStatus.Paid)
                .SumAsync(i => i.FinalAmount);
        }
    }
}
