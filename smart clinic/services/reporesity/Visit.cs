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
        //getallvisits
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

