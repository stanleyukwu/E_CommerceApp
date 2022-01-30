using Rocky_Utility.Models;
using System.Collections.Generic;

namespace Rocky_Model.ViewModel
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }
        public AppUser AppUser { get; set; }
        public IList<Product> ProductList { get; set; }
    }
}
