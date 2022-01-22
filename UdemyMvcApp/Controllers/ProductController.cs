using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using UdemyMvcApp.DataStore;
using UdemyMvcApp.Models;

namespace UdemyMvcApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _ctxt;

        public ProductController(AppDbContext appDbContext)
        {
            _ctxt = appDbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> ProductList = _ctxt.Products;
            foreach(var prodt in ProductList)
            {
                prodt.Category = _ctxt.Categories.FirstOrDefault(x => x.Id == prodt.CategoryId);
            }
            return View(ProductList);
        }

        //Get: UpSert
        public IActionResult UpSert(int? Id)
        {
            IEnumerable<SelectListItem> CategoryDropDown = _ctxt.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewBag.CategoryDropDown = CategoryDropDown;
            Product Product = new Product();
            if(Id == null)
            {
                //this for create
                return View(Product);
            }
            else
            {
                Product = _ctxt.Products.Find(Id);
                if(Product == null)
                {
                    return NotFound();
                }
                return View(Product);
            }            
       
        }

        //Post: UpSert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert()
        {
           
            return View();
        }
    }
}
