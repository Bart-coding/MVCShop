using MVCShop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var dateInThePast = DateTime.Now.AddDays(-7);
            var products = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Date > dateInThePast).Include(p => p.Category);
            return View(await products.ToListAsync());
        }
    }
}