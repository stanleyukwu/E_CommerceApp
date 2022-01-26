using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            IEnumerable<Product> ProductList = _ctxt.Products
                .Include(v => v.Category).Include(v => v.Application).ToList();           
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
                }),
                ApplicationSelectList = _ctxt.Applications.Select(x => new SelectListItem
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

                if(productVM.Product.Id == 0) 
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

                    var productFromDb = _ctxt.Products.AsNoTracking().FirstOrDefault(x => x.Id == productVM.Product.Id);

                    if(files.Count > 0) 
                    { 
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string fileExtension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, productFromDb.Image);

                        if(System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var filestream = new FileStream(Path.Combine(upload, fileName + fileExtension), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        productVM.Product.Image = fileName + fileExtension; 

                    }
                    else
                    {
                        productVM.Product.Image = productFromDb.Image;
                    }
                    _ctxt.Products.Update(productVM.Product);
                }
                _ctxt.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _ctxt.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            productVM.ApplicationSelectList = _ctxt.Applications.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(productVM);
       
        }

        //Get: Delete

        public IActionResult Delete(int? Id)
        {           
            if(Id == null || Id == 0)
            {
                return NotFound();
            }       
            Product ProdTodelete = _ctxt.Products.Include(u => u.Category).Include(u=>u.Application).FirstOrDefault(u => u.Id == Id);                       
            if(ProdTodelete == null)
            {
                return NotFound();
            }
            return View(ProdTodelete);
        }

        //Post: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            var obj = _ctxt.Products.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }
            string upload = _webHostEnv.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

           _ctxt.Products.Remove(obj);
            _ctxt.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
