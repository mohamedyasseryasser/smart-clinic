using Microsoft.AspNetCore.Mvc.Rendering;

namespace smart_clinic.viewmodels.category
{
    public class AddCategoryVM
    {
         public string cat_name { get; set; }
        public string cat_description { get; set; }
        public string? user_id { get; set; }
     }
}
