using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using smart_clinic.enums;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.Appoinment;
using smart_clinic.viewmodels.departmentvm;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Patient;
using System.Threading.Tasks;

namespace smart_clinic.services.reporesity
{
    public class AppoinmentRepo:IAppoinment
    {
        public AppoinmentRepo(Context context,IMapper mapper,IPatient patientservice)
        {
            Context = context;
            Mapper = mapper;
            Patientservice = patientservice;
        }
        public Context Context { get; }
        public IMapper Mapper { get; }
        public IPatient Patientservice { get; }
        //addappoinment
        public async Task<ResponseStatus<ResponseAppoimentVM>> CreateAppoinment(AddAppoinmentVM vm)
        {
            var response =new ResponseStatus<ResponseAppoimentVM>();
            try
            {
                //check appoinmentdate
                var validationvm = new ValidationTimeVM 
                {
                  Appoinmentdate=vm.Appoinmentdate,
                  startat=vm.startat,
                  endat=vm.endat,
                };
                response =await ValidationTime(validationvm);
                if (!response.Success)
                {
                    return response;
                }
                //check patiend
                Patient patient = null;
                if (vm.patientid.HasValue)
                {
                     patient = await Context.Patients.FindAsync(vm.patientid);
                    if (patient == null)
                    {
                        response.Success = false;
                        response.Message = "this patient is not found";
                        response.Errors.Add(response.Message);
                        return response;
                    } 
                }
                else if (!string.IsNullOrEmpty(vm.PhoneNumber))
                {
                    var patientresponse = await Patientservice.getpatientbyphonenumber(vm.PhoneNumber);
                    if (patientresponse.Success!=true)
                    {
                        response.Success = false;
                        response.Message = "this phone number is not found";
                        response.Errors.Add(response.Message);
                        return response;
                    }
                    else
                    {
                        patient =await Context.Patients.FindAsync(patientresponse.Data.patientid);
                        if (patient == null)
                        {
                            response.Success = false;
                            response.Message = " this patient is not found";
                            response.Errors.Add(response.Message);
                            return response;
                        }
                    }
                }
                var appoinment = Mapper.Map<Appoinment>(vm);
                appoinment.status = AppointmentStatus.Pending;
                appoinment.patientid = patient.patientid;
                appoinment.updateat = DateTime.Now;
                await Context.Appointments.AddAsync(appoinment);
                await Context.SaveChangesAsync();
                response.Success=true;
                response.Data = Mapper.Map<ResponseAppoimentVM>(appoinment);
                return 
                    response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //updateappoinment
        public async Task<ResponseStatus<ResponseAppoimentVM>> UpdateAppoinment(UpdateAppoinment vm)
        {
            var response=new ResponseStatus<ResponseAppoimentVM>();
            try
            {
                //check appoinmentid
                var appoinment = await Context.Appointments.FirstOrDefaultAsync(a=>a.appoimentid==vm.AppoinmentId);
                if (appoinment == null) 
                {
                    response.Success=false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                //check appoinmentdate
                var validationvm = new ValidationTimeVM
                {
                    Appoinmentdate = vm.Appoinmentdate,
                    startat = vm.startat,
                    endat = vm.endat,
                };
                response = await ValidationTime(validationvm);
                if (!response.Success)
                {
                    return response;
                }
                appoinment.notes = vm.notes;
                appoinment.resptionistidid=vm.resptionistidid;
                appoinment.doctorid=vm.doctorid;
                appoinment.Appoinmentdate=vm.Appoinmentdate;
                appoinment.startat=vm.startat;
                appoinment.endat=vm.endat;
                await Context.SaveChangesAsync();
                response.Success=true;
                response.Data = Mapper.Map<ResponseAppoimentVM>(appoinment);
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "exit wrong";
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //updateappoinmentstate
        public async Task<ResponseStatus<ResponseAppoimentVM>> UpdateAppoinmentState(UpdateAppoinmentStateVM vm)
        {
            var response = new ResponseStatus<ResponseAppoimentVM>();
            try
            {
                var appoinment =await Context.Appointments.
               Include(a=>a.Patient).
                    FirstOrDefaultAsync(a=>a.appoimentid==vm.AppoinmentId);
                if (appoinment == null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    return response;
                }

                appoinment.status = vm.Status;
                appoinment.updateat = DateTime.Now;
                await Context.SaveChangesAsync();

                response.Success = true;
                response.Data = Mapper.Map<ResponseAppoimentVM>(appoinment);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"wrong : {ex.Message}";
                return response;
            }
            
        }
        //getappoinmentbyid
        public async Task<ResponseStatus<ResponseAppoimentVM>> GetAppoinmentById(int id)
        {
            var response = new ResponseStatus<ResponseAppoimentVM>();
            try
            {
                var appoinment =await Context.Appointments
                                            .Include(a => a.Patient).
                                            Include(a=>a.Doctor).
                                            Include(a=>a.resptionist)
                                            .FirstOrDefaultAsync(a => a.appoimentid == id);

                if (appoinment == null)
                {
                    response.Success = false;
                    response.Message = "not found.";
                    return response;
                }
                response.Success = true;
                response.Data = Mapper.Map<ResponseAppoimentVM>(appoinment);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"wrong: {ex.Message}";
                return response;
            }
        }
        //searchappoinment
        public async Task<ResponseStatus<IEnumerable<ResponseAppoimentVM>>> SearchAppoinment( SearchAppoinmentVM vm)
        {
            var response=new ResponseStatus<IEnumerable<ResponseAppoimentVM>>();
            try
            {
                IQueryable<Appoinment> query = Context.Appointments.
                    Include(a=>a.Doctor).
                    Include(a=>a.resptionist).
                    Include(a=>a.Patient);
                if (!string.IsNullOrEmpty(vm.PhoneNumber))
                {
                    query = query.Where(a=>a.PhoneNumber.ToLower().Contains(vm.PhoneNumber.ToLower()));
                }
                if (vm.Appoinmentdate.HasValue)
                {
                    query = query.Where(a => a.Appoinmentdate==vm.Appoinmentdate);
                }
                if (vm.state.HasValue)
                {
                    query = query.Where(a => a.status==vm.state);
                }
                if (vm.type.HasValue)
                {
                    query = query.Where(a => a.type==vm.type);
                }

               
                var results =await query.ToListAsync();
                response.Success = true;
                response.Data = Mapper.Map<IEnumerable<ResponseAppoimentVM>>(results);
                return response;
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message ="wrong exit";
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //cancelappoinment
        public async Task<ResponseStatus<bool>> CancelAppoinment(int id)
        {
            var response=new ResponseStatus<bool>();
            try
            {
                var appoinment = await Context.Appointments.FirstOrDefaultAsync(a=>a.appoimentid==id);
                if (appoinment==null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                appoinment.status = AppointmentStatus.Cancelled;
                appoinment.updateat= DateTime.Now;
                await Context.SaveChangesAsync();
                response.Success = true;
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
        //getallaponimentbydoctorid
        //getallappoinmentbyreceptionistid
        //validation time
        private async Task<ResponseStatus<ResponseAppoimentVM>> ValidationTime(ValidationTimeVM vm)
        {
            var response=new ResponseStatus<ResponseAppoimentVM>();
            if (vm.Appoinmentdate.Date != DateTime.Now.Date)
            {
                response.Success = false;
                response.Message = "appoinment date must by datetime now";
                response.Errors.Add(response.Message);
                return response;
            }
            //check endat
            if (vm.endat <= vm.startat)
            {
                response.Success = false;
                response.Message = "end date must be greated than start at date";
                response.Errors.Add(response.Message);
                return response;
            }
            //check avaliable datetime
            var check =await Context.Appointments.AnyAsync(a =>
        // same day
        a.Appoinmentdate.Date == vm.Appoinmentdate.Date
        && vm.startat < a.endat
        && vm.endat > a.startat
        &&a.status==AppointmentStatus.Pending);
            if (check)
            {
                response.Success = false;
                response.Message = " this datetime is not avaliable";
                response.Errors.Add(response.Message);
                return response;
            }
            else
            {
                response.Success = true;
                return response;
            }
        } 
    }
}
