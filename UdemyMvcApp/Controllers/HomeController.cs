using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UdemyMvcApp.DataStore;
using UdemyMvcApp.Models;
using UdemyMvcApp.Models.ViewModel;
using UdemyMvcApp.Utility;

namespace UdemyMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _ctxt;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _ctxt = appDbContext;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _ctxt.Products.Include(x => x.Category).Include(x => x.Application),
                Categories = _ctxt.Categories
            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {

            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _ctxt.Products.Include(u => u.Category).Include(u => u.Application)
                          .Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };
            foreach(var item in shoppingCarts)
            {
                if(item.ProductId == id)
                {
                    DetailsVM.ExistsInCart = true;
                }
            }
                 return View(DetailsVM);
        }

        //Adding to Cart
        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCarts.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        //Removing from cart
        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            var ItemToRemove = shoppingCarts.FirstOrDefault(u => u.ProductId == id);
            if(ItemToRemove != null)
            {
                shoppingCarts.Remove(ItemToRemove);
            }
           
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
