using MVCShop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IdentityManager im = new IdentityManager();

        public async Task<ActionResult> Index(int? page)
        {
            var total = db.Products.Where(p => p.Deleted == false).Count();
            var elemsPerSize = 10;
            // wyświetlanie liczby produktów ustawionych w profilu, jeśli użytkownik zalogowany
            if (User.Identity.IsAuthenticated)
            {
                elemsPerSize = im.GetCurentUser().ProductsPerPage;
            }
            var skip = 0;
            if(page != null)
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
                var products = db.Products.Where(p => p.Deleted == false)
                                        .OrderBy(x => x.CategoryID)
                                        .Skip(skip)
                                        .Take(elemsPerSize)
                                        .Include(p => p.Category);

                ViewBag.CurrentPage = page ?? 1;
                ViewBag.NumberOfPages = Math.Ceiling(total / (double)elemsPerSize);

                return View(await products.ToListAsync());
            }
        }
        
        public async Task<ActionResult> RecycleBin(int? page)
        {
            var total = db.Products.Where(p => p.Deleted == true).Count();
            var elemsPerSize = 10;
            // wyświetlanie liczby produktów ustawionych w profilu, jeśli użytkownik zalogowany
            if (User.Identity.IsAuthenticated)
            {
                elemsPerSize = im.GetCurentUser().ProductsPerPage;
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
                var products = db.Products.Where(p => p.Deleted == true)
                                        .OrderBy(x => x.Category.Name.ToLower())
                                        .Skip(skip)
                                        .Take(elemsPerSize)
                                        .Include(p => p.Category);

                ViewBag.CurrentPage = page ?? 1;
                ViewBag.NumberOfPages = Math.Ceiling(total / (double)elemsPerSize);

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
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            //AddProductViewModel model = new AddProductViewModel();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product model, HttpPostedFileBase imageInput)
        {
            Product product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Descritpion = model.Descritpion,
                Date = DateTime.Now,
                VAT = model.VAT,
                Quantity = model.Quantity,
                Visible = model.Visible,
                CategoryID = model.CategoryID
            };
            if (ModelState.IsValid)
            {
                if(imageInput != null)
                {
                    if(imageInput.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("Picture", "Musisz wybrać plik z rozszerzeniem image/jpeg");
                        ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
                        return View(product);
                    }
                    product.Picture = new byte[imageInput.ContentLength];
                    imageInput.InputStream.Read(product.Picture, 0, imageInput.ContentLength);
                }
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,Name,Price,Descritpion,Picture,Date,Discount,VAT,Deleted,Quantity,SalesCounter,Visible,CategoryID")] Product product, HttpPostedFileBase imageInput)
        {
            if (ModelState.IsValid)
            {
                if (imageInput != null)
                {
                    if (imageInput.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("Picture", "Musisz wybrać plik z rozszerzeniem image/jpeg");
                        ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
                        return View(product);
                    }
                    product.Picture = new byte[imageInput.ContentLength];
                    imageInput.InputStream.Read(product.Picture, 0, imageInput.ContentLength);
                }
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("RecycleBin");
        }

        public async Task<ActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            product.Deleted = true;
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RestoreConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            product.Deleted = false;
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("RecycleBin");
        }

        [OverrideAuthorization]
        [AllowAnonymous]
        public async Task<ActionResult> News(int? page)
        {
            var dateInThePast = DateTime.Now.AddDays(-7);
            var total = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Date > dateInThePast)
                                            .OrderByDescending(p => p.Date)
                                            .Take(10)
                                            .Count();
            var elemsPerSize = 10;
            // wyświetlanie liczby produktów ustawionych w profilu, jeśli użytkownik zalogowany
            if (User.Identity.IsAuthenticated)
            {
                elemsPerSize = im.GetCurentUser().ProductsPerPage;
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
                var products = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Date > dateInThePast)
                                            .OrderByDescending(p => p.Date)
                                            .Take(10)
                                            .Skip(skip)
                                            .Take(elemsPerSize)
                                            .Include(p => p.Category);

                // wyświetlanie cen netto, jeśli użytkownik zalogowany i ma tak ustawione w profilu
                if (User.Identity.IsAuthenticated && im.GetCurentUser().Netto)
                {
                    await products.ForEachAsync(p => {
                        if (p.VAT != -1)
                            p.Price -= p.Price * (p.VAT * (decimal)0.01);
                    });
                }

                ViewBag.CurrentPage = page ?? 1;
                ViewBag.NumberOfPages = Math.Ceiling(total / (double)elemsPerSize);

                return View(await products.ToListAsync());
            }
        }

        [OverrideAuthorization]
        [AllowAnonymous]
        public async Task<ActionResult> Sales(int? page)
        {
            var total = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Discount != 0).Count();
            var elemsPerSize = 10;
            // wyświetlanie liczby produktów ustawionych w profilu, jeśli użytkownik zalogowany
            if (User.Identity.IsAuthenticated)
            {
                elemsPerSize = im.GetCurentUser().ProductsPerPage;
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
                var products = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Discount != 0)
                                        .OrderBy(x => x.CategoryID)
                                        .Skip(skip)
                                        .Take(elemsPerSize)
                                        .Include(p => p.Category);

                // wyświetlanie cen netto, jeśli użytkownik zalogowany i ma tak ustawione w profilu
                if (User.Identity.IsAuthenticated && im.GetCurentUser().Netto)
                {
                    await products.ForEachAsync(p => {
                        if (p.VAT != -1)
                            p.Price -= p.Price * (p.VAT * (decimal)0.01);
                    });
                }

                ViewBag.CurrentPage = page ?? 1;
                ViewBag.NumberOfPages = Math.Ceiling(total / (double)elemsPerSize);

                return View(await products.ToListAsync());
            }
        }

        [OverrideAuthorization]
        [AllowAnonymous]
        public async Task<ActionResult> BestSellers(int? page)
        {
            var total = db.Products.Where(p => p.Deleted == false && p.Visible == true)
                                    .OrderByDescending(p => p.SalesCounter)
                                    .Take(10)
                                    .Count();
            var elemsPerSize = 10;
            // wyświetlanie liczby produktów ustawionych w profilu, jeśli użytkownik zalogowany
            if (User.Identity.IsAuthenticated)
            {
                elemsPerSize = im.GetCurentUser().ProductsPerPage;
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
                var products = db.Products.Where(p => p.Deleted == false && p.Visible == true)
                                        .OrderByDescending(p => p.SalesCounter)
                                        .Take(10)
                                        .Skip(skip)
                                        .Take(elemsPerSize)
                                        .Include(p => p.Category);

                // wyświetlanie cen netto, jeśli użytkownik zalogowany i ma tak ustawione w profilu
                if (User.Identity.IsAuthenticated && im.GetCurentUser().Netto)
                {
                    await products.ForEachAsync(p => {
                        if (p.VAT != -1)
                            p.Price -= p.Price * (p.VAT * (decimal)0.01);
                    });
                }

                ViewBag.CurrentPage = page ?? 1;
                ViewBag.NumberOfPages = Math.Ceiling(total / (double)elemsPerSize);

                return View(await products.ToListAsync());
            }
        }

        [OverrideAuthorization]
        [Authorize(Roles ="user")]
        public ActionResult Buy(int? id)
        {
            if (id != null)
                return RedirectToAction("Add", "Cart", new { id });
            else
                return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
