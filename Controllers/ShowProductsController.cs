using MVCShop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCShop.Controllers
{
    public class ShowProductsController : Controller
    {
        // GET: ShowProducts
        private ApplicationDbContext db = new ApplicationDbContext();
        private IdentityManager im = new IdentityManager();

        [Authorize(Roles ="user")]
        public async Task<ActionResult> Index(int? page)
        {
            var user = im.GetCurentUser();
            bool netto = user.Netto; //
            int discount = user.PersonalDiscount; //

            var productsToShow = db.Products.Where(p => p.Deleted == false && p.Visible == true);

            int total = productsToShow.Count();

            var elemsPerSize = 10;
            // wyświetlanie liczby produktów ustawionych w profilu, jeśli użytkownik zalogowany
            if (User.Identity.IsAuthenticated)
            {
                elemsPerSize = user.ProductsPerPage;
            }
            var skip = 0;
            if (page != null)
            {
                skip = (int)(elemsPerSize * (page - 1));
            }
            var canPage = skip < total;

            if (!canPage)
            {
                return View();
            }
            else
            {
                var products = productsToShow.OrderBy(x => x.CategoryID)
                                    .ThenBy(x => x.Price)
                                    .Skip(skip)
                                    .Take(elemsPerSize)
                                    .Include(p => p.Category);

                ViewBag.CurrentPage = page ?? 1;
                ViewBag.NumberOfPages = Math.Ceiling(total / (double)elemsPerSize);

                return View(await products.ToListAsync());
            }
        }

        public ActionResult Buy (int? productId)
        {   //return RedirectToAction("AddToCart", "Cart", {productId});
            //RedirectToAction("AddToCart", new RouteValueDictionary(
            // new { controller = "Cart", action = "AddToCart", productId = 2 }));
            if (productId != null)
                return RedirectToAction("Add", "Cart", new { productId });
            else
               return RedirectToAction("Index");
        }
        
    }
}