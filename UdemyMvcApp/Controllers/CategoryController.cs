using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Rocky_Utility.Models;
using Rocky_DataAccess.DataStore;
using Rocky_DataAccess.DataStore.Repository.Implementation;
using Rocky_DataAccess.DataStore.Repository.Interface;

namespace Rocky_Utility.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _catRepo;
     

        public CategoryController(ICategoryRepo catRepo)
        {
            _catRepo = catRepo;
          
        }
        public IActionResult Index()
        {
            IEnumerable<Category> Categories = _catRepo.GetAll();

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
                _catRepo.Add(cat);
                _catRepo.Save();

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
            var obj = _catRepo.Find(Id.GetValueOrDefault());
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
                _catRepo.Update(cat);
                _catRepo.Save();

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
            var obj = _catRepo.Find(Id.GetValueOrDefault());
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
            var obj = _catRepo.Find(Id.GetValueOrDefault());
           if(obj == null)
            {
                return NotFound();
            }
            _catRepo.Remove(obj);
            _catRepo.Save();
            return RedirectToAction("Index");
           
        }
    }
}
