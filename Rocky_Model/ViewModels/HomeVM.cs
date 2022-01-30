using Rocky_Utility.Models;
using System.Collections.Generic;

namespace Rocky_Model.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
