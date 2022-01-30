
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky_Utility.Models;

namespace Rocky_Model.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicationSelectList { get; set; }
    }
}
