using smart_clinic.enums;
using smart_clinic.viewmodels.Appoinment;

namespace smart_clinic.viewmodels.Visit
{
    public class ResponseVisitVM
    {
        public int visitid { get; set; }
        public string? notes { get; set; }
        public DateTime visitdate { get; set; }
        public string? diagnosis { get; set; }
        public VisitStatus visitstatus { get; set; }
        public int appoinmentid { get; set; }
        public ResponseAppoimentVM? ResponseAppoimentVM { get; set; }
    }
}
