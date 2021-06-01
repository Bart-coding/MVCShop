using MVCShop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var products = db.Products.Where(p => p.Deleted == false).Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        public async Task<ActionResult> RecycleBin()
        {
            var products = db.Products.Where(p => p.Deleted == true).Include(p => p.Category);
            return View(await products.ToListAsync());
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
        public async Task<ActionResult> Create(Product model)
        {
            Product product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Descritpion = model.Descritpion,
                Picture = model.Picture,
                Date = DateTime.Now,
                VAT = model.VAT,
                Quantity = model.Quantity,
                Visible = model.Visible,
                CategoryID = model.CategoryID
            };
            if (ModelState.IsValid)
            {
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
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,Name,Price,Descritpion,Picture,Date,Discount,VAT,Deleted,Quantity,SalesCounter,Visible,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
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

        [AllowAnonymous]
        public async Task<ActionResult> News()
        {
            var dateInThePast = DateTime.Now.AddDays(-7);
            var products = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Date > dateInThePast)
                                        .OrderByDescending(p => p.Date)
                                        .Take(10)
                                        .Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<ActionResult> Sales()
        {
            var products = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Discount != 0)
                                        .Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<ActionResult> MostBought()
        {
            var products = db.Products.Where(p => p.Deleted == false && p.Visible == true)
                                        .OrderByDescending(p=>p.SalesCounter)
                                        .Take(10)
                                        .Include(p => p.Category);

            return View(await products.ToListAsync());
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
