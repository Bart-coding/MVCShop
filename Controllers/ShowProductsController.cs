using MVCShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    public class ShowProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IdentityManager im = new IdentityManager();
        
        public async Task<ActionResult> Index(int? page)
        {
            var user = im.GetCurentUser();
            bool netto;
            int discount;
            if (user!=null)
            {
                netto = user.Netto;
                discount = user.PersonalDiscount;

            }

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

                var dbCategories = await db.Categories.ToListAsync();//
                var dbCategoriesNames = new List<string>();
                ViewBag.categoriesNames = new SelectList(dbCategoriesNames.Distinct());



                return View(await products.ToListAsync());
            }
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null || (User.IsInRole("user") && (product.Deleted || !product.Visible)))
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [Authorize(Roles = "user")]
        public ActionResult Buy (int? id)
        {   
            if (id != null)
                return RedirectToAction("Add", "Cart", new { id });
            else
               return RedirectToAction("Index");
        }
    }
}