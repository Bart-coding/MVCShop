using Microsoft.AspNet.Identity;
using MVCShop.DTO;
using MVCShop.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var orders = db.Orders.Include(o => o.User);
            return View(await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OrderID,State,PaymentMethod,ShippingMethod,UserID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "Id", "Name", order.UserID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Name", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrderID,State,PaymentMethod,ShippingMethod,UserID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Name", order.UserID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [OverrideAuthorization]
        [Authorize]
        public async Task<ActionResult> UserHistory()
        {
            var userId = User.Identity.GetUserId();
            var orders = await db.Orders.Where(o => o.UserID == userId).Include(o => o.User).ToListAsync();
            orders.ForEach(o => 
            {
                o.OrderProducts = db.OrderProducts.Where(op => op.OrderID == o.OrderID)
                                                .Include(x => x.Product)
                                                .ToList();
            });

            return View(orders);
        }

        [OverrideAuthorization]
        [Authorize]
        public ActionResult Place()
        {
            return View(new OrderProductsDto());
        }
        
        [OverrideAuthorization]
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Place([Bind(Include = "PaymentMethod,ShippingMethod")] OrderProductsDto orderProductsDto)
        {
            if (ModelState.IsValid)
            {

                int cartSum = (int) Session["cartSum"];
                // dodanie zamowienia od bazy danych
                Order order = new Order
                {
                    State = "przyjęte",
                    PaymentMethod = orderProductsDto.PaymentMethod,
                    ShippingMethod = orderProductsDto.ShippingMethod,
                    Cost = cartSum,
                    UserID = User.Identity.GetUserId()
                };
                order = db.Orders.Add(order);
                await db.SaveChangesAsync();

                // id produktow, ktore kupil user + ich licznosc
                List<ProductsCountDto> productsCounts = (List<ProductsCountDto>)Session["cart"];

                
                var products = new List<Product>();
                
                productsCounts.ForEach(p => products.Add(db.Products.Find(p.ProductID)));
                

                foreach(var product in products)
                {
                    // ile tych produktow kupil user
                    var numberOfProducts = productsCounts.Find(x => x.ProductID == product.ProductID).Count;

                    product.SalesCounter += numberOfProducts;
                    product.Quantity -= numberOfProducts;

                    // modyfikacja produktu w bazie danych
                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    // dodanie rekordu tabeli lacznikowej
                    OrderProduct orderProduct = new OrderProduct
                    {
                        OrderID = order.OrderID,
                        ProductID = product.ProductID,
                        NumberOfProducts = numberOfProducts
                    };
                    db.OrderProducts.Add(orderProduct);
                    await db.SaveChangesAsync();
                }
                Session["cart"] = null;
                Session["cartSum"] = null;
                return RedirectToAction("Index", "Home");
            }
            return View();
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
