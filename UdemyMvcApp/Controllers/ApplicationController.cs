using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Rocky_Utility.Models;
using Rocky_DataAccess.DataStore;
using Rocky_DataAccess.DataStore.Repository.Interface;

namespace Rocky_Utility.Controllers
{
    public class ApplicationController : Controller
    {
    
        private IApplicationRepo _app;

        public ApplicationController(IApplicationRepo app)
        {
            _app = app;

        }
        public IActionResult Index()
        {
            IEnumerable<Application> Applications = _app.GetAll();
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
            _app.Add(app);
            _app.Save();

            return RedirectToAction("Index");
            }

        //Get: Edit
        public IActionResult Edit(int? Id)
        {
            if(Id == null || Id < 1)
            {
                return NotFound();
            }
            var app = _app.Find(Id.GetValueOrDefault());
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
                _app.Update(app);
                _app.Save();

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
            var app = _app.Find(Id.GetValueOrDefault());
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
            var obj = _app.Find(Id.GetValueOrDefault());
            if(obj != null)
            {
                _app.Remove(obj);
                _app.Save();
                return RedirectToAction("Index");
            }
            return NotFound();


        }
    }
}
