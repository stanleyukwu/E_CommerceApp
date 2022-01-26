using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UdemyMvcApp.DataStore;
using UdemyMvcApp.Models;

namespace UdemyMvcApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _ctxt;

        public CategoryController(AppDbContext appDbContext)
        {
            _ctxt = appDbContext;
        }
        public IActionResult Index()
        {
            List<Category> Categories = _ctxt.Categories.ToList(); ;

            return View(Categories);
        }
        //Get: Create

        public IActionResult Create()
        {
            return View();
        }

        //Post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                _ctxt.Categories.Add(cat);
                _ctxt.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(cat);
        }

        //Get: Edit

        public IActionResult Edit(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _ctxt.Categories.Find(Id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _ctxt.Categories.Update(cat);
                _ctxt.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(cat);
        }

        //Get: Delete

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _ctxt.Categories.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            var obj = _ctxt.Categories.Find(Id);
           if(obj == null)
            {
                return NotFound();
            }
                _ctxt.Categories.Remove(obj);
                _ctxt.SaveChanges();
            return RedirectToAction("Index");
           
        }
    }
}
