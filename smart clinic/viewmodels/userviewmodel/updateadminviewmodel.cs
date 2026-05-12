using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.userviewmodel
{
    public class updateadminviewmodel:updateuserviewmodel
    {
        
        [MinLength(3)]
        public string permissions { get; set; }
    }
}
