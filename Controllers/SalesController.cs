using MVCShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var products = db.Products.Where(p => (p.Deleted == false && p.Visible == true && p.Discount != 0)).Include(p => p.Category);
            return View(await products.ToListAsync());
        }
    }
}