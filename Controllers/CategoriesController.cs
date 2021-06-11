using iTextSharp.text;
using iTextSharp.text.pdf;
using MVCShop.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var categories = db.Categories.Include(c => c.CategoryType);
            return View(await categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryTypeID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryID,Name,Visible,CategoryTypeID")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryTypeID = new SelectList(db.Categories, "CategoryID", "Name", category.CategoryTypeID);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryTypeID = new SelectList(db.Categories, "CategoryID", "Name", category.CategoryTypeID);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryID,Name,Visible,CategoryTypeID")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryTypeID = new SelectList(db.Categories, "CategoryID", "Name", category.CategoryTypeID);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult GeneratePriceList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["CategoryID"] = id;
            return CreatePdf();
        }

        public FileResult CreatePdf()
        {
            var CategoryID = int.Parse(TempData["CategoryID"].ToString());

            MemoryStream workStream = new MemoryStream();

            //nazwa pdfa
            string strPDFFileName = string.Format("Cennik ({0}).pdf", db.Categories.Find(CategoryID).Name);

            Document doc = new Document();
            doc.SetMargins(5f, 5f, 0f, 0f);

            //tworzenie tabeli z liczbą kolumn 3
            PdfPTable tableLayout = new PdfPTable(3);

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            // dodanie zawartości pdfa   
            doc.Add(AddContentToPDF(tableLayout));

            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable AddContentToPDF(PdfPTable tableLayout)
        {

            float[] headers = { 60, 25, 15 }; // szerokości kolumn  
            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 100; // szerokość pdfa na 100% 
            tableLayout.HeaderRows = 1; // liczba nagłówków

            var CategoryID = int.Parse(TempData["CategoryID"].ToString());

            // specjalna czcionka uwzględniająca polskie znaki
            var titleFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 22, Font.BOLD, BaseColor.BLACK);

            // Dodanie tytułu pdfa na samej górze
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Format("Cennik ({0})", db.Categories.Find(CategoryID).Name), titleFont))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 15,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            // dodanie nagłówków 
            AddCellToHeader(tableLayout, "Nazwa");
            AddCellToHeader(tableLayout, "Cena");
            AddCellToHeader(tableLayout, "Ilość");

            // iteracja po liscie i dodanie poszczególnych wierszy
            List<Product> products = db.Products.Where(p => p.CategoryID == CategoryID && p.Deleted == false).ToList();
            foreach (var product in products)
            {
                AddCellToBody(tableLayout, product.Name);
                AddCellToBody(tableLayout, product.Price.ToString());
                AddCellToBody(tableLayout, product.Quantity.ToString());
            }

            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            // specjalna czcionka uwzględniająca polskie znaki
            var headerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 18, Font.BOLD, BaseColor.WHITE);
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, headerFont))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new BaseColor(0, 125, 160)
            });
        }

        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            // specjalna czcionka uwzględniająca polskie znaki
            var itemFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 8, 1, BaseColor.BLACK);
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, itemFont))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new BaseColor(255, 255, 255)
            });
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
