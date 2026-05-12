using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.departmentvm;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Patient;
using System.Threading.Tasks;

namespace smart_clinic.services.reporesity
{
    public class PatientRepo:IPatient
    {
        public PatientRepo(Context context,IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public Context Context { get; }
        public IMapper Mapper { get; }

        //createpatient
        public async Task<ResponseStatus<ResponsePatientVM>> CreatePatient(AddPatientVM vm)
        {
            var response=new ResponseStatus<ResponsePatientVM>();
            try
            {
                if (vm.nationalid.HasValue)
                {
                    if (await exitnationalid(vm.nationalid.Value))
                    {
                        response.Success = false;
                        response.Message = "this nationalid is already exit";
                        response.Errors.Add(response.Message);
                        return response;
                    }
                }
                if (await exitphonenumber(vm.phonenumber))
                {
                    response.Success = false;
                    response.Message = "this phone is already exit";
                    response.Errors.Add(response.Message);
                    return response;
                }
                var patient =Mapper.Map<Patient>(vm);
                patient.isvalid=true;   
                await Context.Patients.AddAsync(patient);
                await Context.SaveChangesAsync();
                response.Success = true;
                response.Message = "created patient successfully";
                response.Data = Mapper.Map<ResponsePatientVM>(patient);
                 return response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = "wrong";
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //update
        public async Task<ResponseStatus<ResponsePatientVM>> UpdatePatient(UpdatePatientVM vm)
        {
            var response=new ResponseStatus<ResponsePatientVM>();
            try
            {
                var patient=await Context.Patients.FirstOrDefaultAsync(p=>p.patientid==vm.patientid);
                if (patient==null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                if (vm.nationalid.HasValue)
                {
                    if (vm.nationalid.Value != patient.nationalid.Value &&await exitnationalid(vm.nationalid.Value))
                    {
                        response.Success = false;
                        response.Message = "this nationalid is already exit";
                        response.Errors.Add(response.Message);
                        return response;
                    }
                }
                if (vm.phonenumber != patient.phonenumber && await exitphonenumber(vm.phonenumber))
                {
                    response.Success = false;
                    response.Message = "this phone is already exit";
                    response.Errors.Add(response.Message);
                    return response;
                }
                patient.datebirth = vm.datebirth;
                patient.phonenumber=vm.phonenumber;
                patient.patientname = vm.patientname;
                patient.Gender = vm.Gender;
                patient.isvalid = vm.isvalid;
                patient.nationalid = vm.nationalid;
                await Context.SaveChangesAsync();
                response.Success=true;
                response.Data=Mapper.Map<ResponsePatientVM>(vm);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "exit wrong";
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //remove
        public async Task<ResponseStatus<bool>> RemovePatient(int id)
        {
            var response = new ResponseStatus<bool>();

            try
            {
                var patient =await Context.Patients.FirstOrDefaultAsync(p => p.patientid == id);
                if (patient == null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    response.Data = false;
                    return response;
                }
                patient.isvalid = false;
                await Context.SaveChangesAsync();
                response.Success=true;
                response.Data = true;
                return response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = "not found";
                response.Errors.Add(response.Message);
                response.Data = false;
                return response;
            }
        }
        //getpatientbyphonenumber
        public async Task<ResponseStatus<ResponsePatientVM>> getpatientbyphonenumber(string phone)
        {
            var response=new ResponseStatus<ResponsePatientVM>();
            try
            {
                var patient = await Context.Patients.
                    FirstOrDefaultAsync(p=>p.phonenumber.ToLower()==phone.ToLower());
                if (patient==null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                     return response;
                }
                response.Success = true;
                response.Data = Mapper.Map<ResponsePatientVM>(patient); ;
                return response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = "exit wrong";
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //getpatientbyid
        public async Task<ResponseStatus<ResponsePatientVM>> getpatientbyid(int id)
        {
            var response = new ResponseStatus<ResponsePatientVM>();
            try
            {
                var patient = await Context.Patients.
                    FirstOrDefaultAsync(p => p.patientid == id);
                if (patient == null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                response.Success = true;
                response.Data = Mapper.Map<ResponsePatientVM>(patient); ;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "exit wrong";
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //getbyname
        public async Task<ResponseStatus<ResponsePatientVM>> getpatientbyname(string name)
        {
            var response = new ResponseStatus<ResponsePatientVM>();
            try
            {
                var patient = await Context.Patients.
                    FirstOrDefaultAsync(p => p.phonenumber.ToLower().Contains(name.ToLower()));
                if (patient == null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                response.Success = true;
                response.Data = Mapper.Map<ResponsePatientVM>(patient); ;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "exit wrong";
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //getall
        public async Task<ResponseStatus<IEnumerable<ResponsePatientVM>>> GetAllAsync(pagination pg, 
            string? patientname=null,string? phone=null,long? nationalid=null,bool? isactive=null)
        {
            var response = new ResponseStatus<IEnumerable<ResponsePatientVM>>();
            try
            {
                IQueryable<Patient> query = Context.Patients.AsNoTracking();
                if (!string.IsNullOrEmpty(patientname))
                {
                    query = query.Where(p=>p.patientname.ToLower().Contains(patientname.ToLower()));
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    query = query.Where(p => p.phonenumber.ToLower().Contains(phone.ToLower()));

                }
                if (nationalid.HasValue&&nationalid!=null)
                {
                    query = query.Where(p => p.nationalid==nationalid);
                }
                if (isactive.HasValue&&isactive!=null)
                {
                    query = query.Where(p => p.isvalid.ToString().ToLower()==isactive.ToString().ToLower());
                }

                var totalCount = await query.CountAsync();
                var patients = await query.OrderByDescending(p => p.patientid)
                                         .Skip((pg.PageNumber - 1) * pg.PageSize)
                                         .Take(pg.PageSize)
                                         .ToListAsync();

                response.Success = true;
                response.Data = Mapper.Map<IEnumerable<ResponsePatientVM>>(patients);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving patients";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
        public async Task<ResponseStatus<ResponsePatientVM>> GetPatientDetailsAsync(int id)
        {
            var response = new ResponseStatus<ResponsePatientVM>();
            try
            {
                var patient = await Context.Patients
                    .Include(p => p.appoinments)
                        .ThenInclude(a => a.Doctor)
                    .Include(p => p.appoinments)
                        .ThenInclude(a => a.Visit)
                            .ThenInclude(v => v.Prescription)
                                .ThenInclude(pr => pr.items)
                                    .ThenInclude(i => i.Medicine)
                    .Include(p => p.appoinments)
                        .ThenInclude(a => a.Visit)
                            .ThenInclude(v => v.Invoice)
                    .FirstOrDefaultAsync(p => p.patientid == id);

                if (patient == null)
                {
                    response.Success = false;
                    response.Message = "Patient not found";
                    return response;
                }

                var vm = Mapper.Map<ResponsePatientVM>(patient);

                // Manual mapping for nested data to ensure clarity
                vm.Appointments = patient.appoinments.Select(a => new AppointmentResponseVM
                {
                    appoimentid = a.appoimentid,
                    Appoinmentdate = a.Appoinmentdate,
                    startat = a.startat,
                    endat = a.endat,
                    notes = a.notes,
                    status = a.status,
                    DoctorName = a.Doctor?.user?.UserName ?? "N/A"
                }).ToList();

                vm.Visits = patient.appoinments.Where(a => a.Visit != null).Select(a => new VisitResponseVM
                {
                    visitid = a.Visit.visitid,
                    visitdate = a.Visit.visitdate,
                    diagnosis = a.Visit.diagnosis,
                    notes = a.Visit.notes,
                    visitstatus = a.Visit.visitstatus
                }).ToList();

                vm.Prescriptions = patient.appoinments.Where(a => a.Visit?.Prescription != null).Select(a => new PrescriptionResponseVM
                {
                    prescriptionid = a.Visit.Prescription.prescriptionid,
                    prescriptiondate = a.Visit.Prescription.prescriptiondate,
                    notes = a.Visit.Prescription.notes,
                    Medicines = a.Visit.Prescription.items.Select(i => i.Medicine?.Name).ToList()
                }).ToList();

                vm.Invoices = patient.appoinments.Where(a => a.Visit?.Invoice != null).Select(a => new InvoiceResponseVM
                {
                    InvoiceId = a.Visit.Invoice.InvoiceId,
                    FinalAmount = a.Visit.Invoice.FinalAmount,
                    CreatedAt = a.Visit.Invoice.CreatedAt,
                    Status = a.Visit.Invoice.Status
                }).ToList();

                response.Success = true;
                response.Data = vm;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving patient details";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
        private async Task<bool> exitphonenumber(string phone)
        {
            return await Context.Patients.AnyAsync(p => p.phonenumber == phone);
        }
        private async Task<bool> exitnationalid(long id)
        {
            return await Context.Patients.AnyAsync(p => p.nationalid == id);
        }

       
    }
}
