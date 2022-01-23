using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UdemyMvcApp.DataStore;
using UdemyMvcApp.Models;
using UdemyMvcApp.Models.ViewModel;

namespace UdemyMvcApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _ctxt;
        private readonly IWebHostEnvironment _webHostEnv;

        public ProductController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _ctxt = appDbContext;
            _webHostEnv = webHostEnvironment;
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
            //IEnumerable<SelectListItem> CategoryDropDown = _ctxt.Categories.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;
            //Product Product = new Product();

            ProductVM ProductVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _ctxt.Categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
           
            if(Id == null)
            {
                //this for create
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = _ctxt.Products.Find(Id);
                if(ProductVM == null)
                {
                    return NotFound();
                }
                return View(ProductVM);
            }            
       
        }

        //Post: UpSert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var webRootPath = _webHostEnv.WebRootPath;

                if(productVM.Product.Id != 0) 
                { 
                    //we are creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string fileExtension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(upload, fileName + fileExtension),FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    productVM.Product.Image = fileName + fileExtension;

                    _ctxt.Products.Add(productVM.Product);
                }
                else
                {
                    //we are updating
                }
                _ctxt.SaveChanges();
            }
           
            return View();
        }
    }
}
