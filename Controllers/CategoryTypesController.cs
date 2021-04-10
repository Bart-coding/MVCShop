using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCShop.Models;

namespace MVCShop.Controllers
{
    public class CategoryTypesController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: CategoryTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.CategoryTypes.ToListAsync());
        }

        // GET: CategoryTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryType categoryType = await db.CategoryTypes.FindAsync(id);
            if (categoryType == null)
            {
                return HttpNotFound();
            }
            return View(categoryType);
        }

        // GET: CategoryTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryTypeID,Name")] CategoryType categoryType)
        {
            if (ModelState.IsValid)
            {
                db.CategoryTypes.Add(categoryType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(categoryType);
        }

        // GET: CategoryTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryType categoryType = await db.CategoryTypes.FindAsync(id);
            if (categoryType == null)
            {
                return HttpNotFound();
            }
            return View(categoryType);
        }

        // POST: CategoryTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryTypeID,Name")] CategoryType categoryType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoryType);
        }

        // GET: CategoryTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryType categoryType = await db.CategoryTypes.FindAsync(id);
            if (categoryType == null)
            {
                return HttpNotFound();
            }
            return View(categoryType);
        }

        // POST: CategoryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CategoryType categoryType = await db.CategoryTypes.FindAsync(id);
            db.CategoryTypes.Remove(categoryType);
            await db.SaveChangesAsync();
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
