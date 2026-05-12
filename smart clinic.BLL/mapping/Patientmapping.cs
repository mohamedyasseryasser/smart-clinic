using AutoMapper;
using smart_clinic.Models;
using smart_clinic.viewmodels.Patient;

namespace smart_clinic.mapping
{
    public class Patientmapping:Profile
    {
       public Patientmapping()
        {
            CreateMap<AddPatientVM, Patient>();
            CreateMap<Patient, ResponsePatientVM>();
            CreateMap<UpdatePatientVM, ResponsePatientVM>();

        }
    }
}
