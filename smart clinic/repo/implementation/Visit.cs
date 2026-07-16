using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smart_clinic.enums;
using smart_clinic.Models;
using smart_clinic.services.interfaces;
using smart_clinic.viewmodels.General;
using smart_clinic.viewmodels.Visit;
using smart_clinic.viewmodels.VisitRepo;
using System.Threading.Tasks;

namespace smart_clinic.services.reporesity
{
    public class VisitRepo:IVisit
    {
        public VisitRepo(Context context,IMapper mapper) 
        {
            Context = context;
            Mapper = mapper;
        }
        public Context Context { get; }
        public IMapper Mapper { get; }

        //create visit
        public async Task<ResponseStatus<ResponseVisitVM>> createvisit(AddVisit vm)
        {
            var response=new ResponseStatus<ResponseVisitVM>();
            try
            {
                var appoinment = await Context.Appointments.FindAsync(vm.appoinmentid);
                if (appoinment == null) 
                {
                    response.Success = false;
                    response.Message = "appoinment is not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                var visit = Mapper.Map<Visit>(vm);
                visit.appoinmentid = vm.appoinmentid;
                visit.visitdate = DateTime.Now;
                visit.visitstatus = VisitStatus.InProgress;
                await Context.Visits.AddAsync(visit);
                await Context.SaveChangesAsync();
                response.Success = true;
                response.Data = Mapper.Map<ResponseVisitVM>(visit);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Errors.Add(response.Message);
                return response;
            }
        }
        //getvisitbyid
        public async Task<ResponseStatus<ResponseVisitVM>> getvisitbyid(int id)
        {
            var response = new ResponseStatus<ResponseVisitVM>();
            try
            {
                //check visitid
                var vistt=await Context.Visits.
                    Include(v=>v.Appoinment).
                    ThenInclude(a=>a.Patient).
                    FirstOrDefaultAsync(v=>v.visitid==id);
                if(vistt == null) 
                {
                   response.Success=false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                response.Success=true;
                response.Data=Mapper.Map<ResponseVisitVM>(vistt);  
                return response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = $"exit wrong: {ex.Message}";
            }
            return response;
        }
        //updatevisit
        public async Task<ResponseStatus<ResponseVisitVM>> updatevisit(UpdateVisitVM vm)
        {
            var response=new ResponseStatus<ResponseVisitVM>();
            try
            {
                //check visitid
                var vistt =await Context.Visits.
                    FirstOrDefaultAsync(v => v.visitid == vm.visitid);
                if (vistt == null)
                {
                    response.Success = false;
                    response.Message = "not found";
                    response.Errors.Add(response.Message);
                    return response;
                }
                vistt.visitdate = DateTime.Now;
                vistt.notes = vm.notes;
                vistt.visitstatus = vm.visitstatus;
                vistt.diagnosis = vm.diagnosis;
                await Context.SaveChangesAsync();
                response.Success=true;
                response.Data = Mapper.Map<ResponseVisitVM>(vm);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "exit wrong";
            }
            return response;
        }
        //deletevisit
        public async Task<ResponseStatus<bool>> deletevisit(int id)
        {
            var response = new ResponseStatus<bool>();
            try
            {
                var visit=await Context.Visits.FindAsync(id);
                if (visit==null)
                {
                    response.Success=false;
                    response.Message = "visit is not found";
                    response.Errors.Add(response.Message);
                    response.Data = false;
                    return response;
                }
                Context.Visits.Remove(visit);
                await Context.SaveChangesAsync();
                response.Success = true;
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"{ex.Message}";
                response.Errors.Add(response.Message);
                response.Data = false;
                return response;
            }
        }
        //cancelvisit
        public async Task<ResponseStatus<bool>> cancelvisit(int id)
        {
            var response = new ResponseStatus<bool>();
            using var transaction = await Context.Database.BeginTransactionAsync();

            try
            {
                var visit = await Context.Visits
                    .Include(v => v.Appoinment)
                    .FirstOrDefaultAsync(v => v.visitid == id);

                if (visit == null)
                {
                    response.Success = false;
                    response.Message = "Visit not found";
                    return response;
                }
 
                if (visit.Appoinment != null)
                {
                    visit.Appoinment.status = AppointmentStatus.Confirmed;
                }

                Context.Visits.Remove(visit);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Visit canceled successfully";

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                response.Success = false;
                response.Message = "An error occurred while canceling the visit";

                return response;
            }
        }
        //completevisit
        public async Task<ResponseStatus<ResponseVisitVM>> completevisit(int id)
        {
            var response = new ResponseStatus<ResponseVisitVM>();
            using var transaction = await Context.Database.BeginTransactionAsync();

            try
            {
                var visit = await Context.Visits
                    .Include(v => v.Appoinment)
                    .FirstOrDefaultAsync(v => v.visitid == id);

                if (visit == null)
                {
                    response.Success = false;
                    response.Message = "Visit not found";
                    return response;
                }

                visit.visitstatus = VisitStatus.Completed;

                if (visit.Appoinment != null)
                {
                    visit.Appoinment.status = AppointmentStatus.Completed;
                }

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Success = true;
                response.Message = "Visit completed successfully";
                response.Data = Mapper.Map<ResponseVisitVM>(visit);

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                response.Success = false;
                response.Message = "An error occurred while completing the visit";

                return response;
            }
        }
        //getall
        public async Task<ResponseStatus<IEnumerable<ResponseVisitVM>>> GetAllVisits()
        {
            var response = new ResponseStatus<IEnumerable<ResponseVisitVM>>();
            try
            {
                var visits = await Context.Visits
                    .Include(v => v.Appoinment)
                    .ThenInclude(a => a.Patient)
                    .OrderByDescending(v => v.visitid)
                    .ToListAsync();

                response.Success = true;
                response.Data = Mapper.Map<IEnumerable<ResponseVisitVM>>(visits);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"حدث خطأ: {ex.Message}";
                return response;
            }
        }
        //displayallvisits
        public  async Task<ResponseStatus<IEnumerable<ResponseVisitVM>>> getallvisit(DateTime date ,pagination pg)
        {
            var response= new ResponseStatus<IEnumerable<ResponseVisitVM>>();
            try
            {
                IQueryable<Visit> query = Context.Visits.
                    Include(v => v.Appoinment)
                    .ThenInclude(a=>a.Patient).Where(v=>v.visitdate.Date==date.Date);
               
                var visits = await query.OrderByDescending(p => p.visitid)
                                                      .Skip((pg.PageNumber - 1) * pg.PageSize)
                                                      .Take(pg.PageSize)
                                                      .ToListAsync(); 

                response.Success = true;
                response.Data = Mapper.Map<IEnumerable<ResponseVisitVM>>(visits);

                return response;

            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message = $"exit wrong {ex.Message}";
                return response;
            }
        }
    }
}