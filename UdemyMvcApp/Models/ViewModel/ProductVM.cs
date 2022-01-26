using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace UdemyMvcApp.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicationSelectList { get; set; }
    }
}
