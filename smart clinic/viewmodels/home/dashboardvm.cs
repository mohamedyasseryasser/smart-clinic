using smart_clinic.viewmodels.Appoinment;

namespace smart_clinic.viewmodels.home
{
    public class dashboardvm
    {
        public int patientcount {  get; set; }
        public int appoinmentcount {  get; set; }
        public int totaldoctors {  get; set; }
        public decimal revenue {  get; set; }
        public List<ResponseAppoimentVM> items { get; set; }=new List<ResponseAppoimentVM>();
    }
}
