using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UdemyMvcApp.DataStore;
using UdemyMvcApp.Models;
using UdemyMvcApp.Models.ViewModel;
using UdemyMvcApp.Utility;

namespace UdemyMvcApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _ctxt;

        public IWebHostEnvironment _webHostEnv { get; }

        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController( AppDbContext appDbContext, IWebHostEnvironment webhostEnvironment, IEmailSender emailSender)
        {
            _ctxt = appDbContext;
            _webHostEnv = webhostEnvironment;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).ToList();
            }
            List<int> ProductInCart = shoppingCartList.Select(u => u.ProductId).ToList();
            List<Product> ProductIncartList = _ctxt.Products.Where(u => ProductInCart.Contains(u.Id)).ToList();
            return View(ProductIncartList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            // var path = _webHostEnv.WebRootPath + Path.DirectorySeparatorChar.ToString() +
            //"Templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            var Path = Directory.GetCurrentDirectory();
            var dir = "\\wwwroot\\Temlates\\Inquiry.html";
            var fullPath = Path + dir;

            var subject = "New Inquiry";
            var HtmlBody = "";
            using(StreamReader sr = System.IO.File.OpenText(fullPath))
            {
                HtmlBody = sr.ReadToEnd();
            }
            StringBuilder ProductListSB = new StringBuilder();
            foreach(var prod in ProductUserVM.ProductList)
            {
                ProductListSB.Append($"_ Name:{prod.Name} <span style='font-size:14px'> (ID:{prod.Id})</span> <br/> ");
            }
            string messageBody = string.Format(HtmlBody,
                ProductUserVM.AppUser.FullName,
                ProductUserVM.AppUser.Email,
                ProductUserVM.AppUser.PhoneNumber,
                ProductListSB.ToString());

            await _emailSender.SendEmailAsync(WC.AdminEmail, subject, messageBody);
            return RedirectToAction(nameof(InquiryConfiirmation));
        }

        public IActionResult InquiryConfiirmation(ProductUserVM ProductUserVM)
        {
            HttpContext.Session.Clear();
            return View();
        }
    
        public IActionResult Summary()
         {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // or
            //var UserId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).ToList();
            }
            List<int> ProductInCart = shoppingCartList.Select(u => u.ProductId).ToList();
            List<Product> ProductIncartList = _ctxt.Products.Where(u => ProductInCart.Contains(u.Id)).ToList();

            ProductUserVM ProductUserVM = new ProductUserVM()
            {
                AppUser = _ctxt.AppUsers.FirstOrDefault(u => u.Id == claims.Value),
                ProductList = ProductIncartList
            };

            return View(ProductUserVM);
        }

       public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).ToList();
            }
            var ProdToDelete = shoppingCartList.FirstOrDefault(u => u.ProductId == id);
            shoppingCartList.Remove(ProdToDelete);
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
