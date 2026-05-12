using smart_clinic.enums;
using System.ComponentModel.DataAnnotations;

namespace smart_clinic.viewmodels.userviewmodel
{
    public class adminviewmodel : userviewmodel
    {
        [Required]
        [MinLength(4,ErrorMessage ="permission must by 4 char minemim")]
        public string permissions { get; set; }
    }
}
