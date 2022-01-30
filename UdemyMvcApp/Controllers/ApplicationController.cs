using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Rocky_Utility.Models;
using Rocky_DataAccess.DataStore;

namespace Rocky_Utility.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly AppDbContext _cxt;

        public ApplicationController(AppDbContext appDbContext)
        {
            _cxt = appDbContext;
        }
        public IActionResult Index()
        {
            List<Application> Applications = _cxt.Applications.ToList();
            return View(Applications);
        }

        //Get: Create
        public IActionResult Create()
        {
            return View();
        }

        //Post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
            public IActionResult Create(Application app)
            {
            _cxt.Applications.Add(app);
            _cxt.SaveChanges();

            return RedirectToAction("Index");
            }

        //Get: Edit
        public IActionResult Edit(int? Id)
        {
            if(Id == null || Id < 1)
            {
                return NotFound();
            }
            var app = _cxt.Applications.Find(Id);
            if(app == null)
            {
                return NotFound();
            }
            return View(app);
        }

        //Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Application app)
        {
            if (ModelState.IsValid)
            {
                _cxt.Applications.Update(app);
                _cxt.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(app);
        }

        //Get: Delete
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id < 1)
            {
                return NotFound();
            }
            var app = _cxt.Applications.Find(Id);
            if (app == null)
            {
                return NotFound();
            }
            return View(app);
        }

        //Post: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            var obj = _cxt.Applications.Find(Id);
            if(obj != null)
            {
                _cxt.Applications.Remove(obj);
                _cxt.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();


        }
    }
}
