using smart_clinic.enums;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.userviewmodel
{
    public class responseadminviewmodel:responseuserviewmodel
    {
        public int adminid {  get; set; }
        public string permissions { get; set; }
        public userstatus status { get; set; }  
    }
}
