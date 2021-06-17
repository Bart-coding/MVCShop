using MVCShop.DTO;
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
    [Authorize(Roles = "user")] //
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private IdentityManager im = new IdentityManager();

        // GET: Cart
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (Session["cart"] == null)
            {
                return View();
            }
            if (((List<ProductsCountDto>)Session["cart"]).Count == 0)
            {
                return View();
            }

            List<ProductsCountDto> cartProducts = (List<ProductsCountDto>)Session["cart"];

            List<Product> listToShow = new List<Product>();
            List<int> productsQuantities = new List<int>();
            double priceSum = 0;
            Product tempProduct;
            int tempProductID, tempQuantity;
            for (int i = 0; i < cartProducts.Count; i++)
            {
                tempProductID = cartProducts[i].ProductID;
                tempProduct = await db.Products.FindAsync(tempProductID);
                listToShow.Add(tempProduct);
                tempQuantity = cartProducts[i].Count;
                productsQuantities.Add(tempQuantity);
                priceSum += Math.Round((double)((tempProduct.Price - ((tempProduct.Price* tempProduct.Discount)/100)) * tempQuantity),2);

            }

            ViewBag.productsQuantities = productsQuantities;
            ViewBag.priceSum = priceSum;
            Session["cartSum"] = priceSum;
                
            return View(listToShow);
        }

        public async Task<ActionResult> Add(int? id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null || product.Deleted || !product.Visible)
                return HttpNotFound();

            if (id != null)
            {
                List<ProductsCountDto> cartProducts;
                if (Session["cart"] == null)
                {
                    cartProducts = new List<ProductsCountDto>();
                }

                else
                {
                    cartProducts = (List<ProductsCountDto>)Session["cart"];
                    int productIndex = cartProducts.FindIndex(p => p.ProductID == id);
                    if (productIndex > -1)
                    {
                        var productDTO = cartProducts[productIndex];
                        productDTO.Count++;
                        cartProducts[productIndex] = productDTO;
                        return RedirectToAction("Index");
                    }

                }

                cartProducts.Add(new ProductsCountDto { ProductID = (int)id, Count = 1 });

                Session["cart"] = cartProducts;
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return RedirectToAction("Index");

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            List<ProductsCountDto> cartProducts;
            if (Session["cart"] == null)
                return View();//

            cartProducts = (List<ProductsCountDto>)Session["cart"];

            int productIndex = cartProducts.FindIndex(p => p.ProductID == id);
            if (productIndex > -1)
            {
                cartProducts.RemoveAt(productIndex);
                Session["cart"] = cartProducts;
            }
            else
            {
                return HttpNotFound();
            }
                

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = await db.Products.FindAsync(id);
            if (product == null || product.Deleted || !product.Visible)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Increase (int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (Session["cart"] == null || ((List<ProductsCountDto>) Session["cart"]).Count == 0)
                return RedirectToAction("Index");

            List<ProductsCountDto> cartProducts = (List<ProductsCountDto>) Session["cart"];

            int productIndex = cartProducts.FindIndex(p => p.ProductID == id);

            if (productIndex!=-1)
            {
                var product = cartProducts[productIndex];
                product.Count++;
                cartProducts[productIndex] = product;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Decrease(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (Session["cart"] == null || ((List<ProductsCountDto>)Session["cart"]).Count == 0)
                return RedirectToAction("Index");

            List<ProductsCountDto> cartProducts = (List<ProductsCountDto>)Session["cart"];

            int productIndex = cartProducts.FindIndex(p => p.ProductID == id);

            if (productIndex > -1)
            {
                if (cartProducts[productIndex].Count > 1)
                {
                    var product = cartProducts[productIndex];
                    product.Count--;
                    cartProducts[productIndex] = product;
                }
                else if (cartProducts[productIndex].Count == 1)
                {
                    cartProducts.RemoveAt(productIndex);
                }

                Session["cart"] = cartProducts;
            }
                
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Order()
        {
            if(Session["cart"] == null)
            {
                return View();
            }
            if (((List<ProductsCountDto>)Session["cart"]).Count == 0)
            {
                return View();
            }

            List<ProductsCountDto> cartProducts = (List<ProductsCountDto>)Session["cart"];
            
            OrderProduct tempOrderProduct;
            Product tempProduct;

            List<OrderProduct> orderProducts = new List<OrderProduct>();

            
            foreach (ProductsCountDto cartProduct in cartProducts)
            {
                tempOrderProduct = new OrderProduct();
                tempOrderProduct.ProductID = cartProduct.ProductID;
                tempOrderProduct.NumberOfProducts = cartProduct.Count;
                tempProduct = await db.Products.FindAsync(cartProduct.ProductID);
                tempOrderProduct.Product = tempProduct;
            }

            Order newOrder = new Order();

            newOrder.State = "nowy";
            newOrder.OrderProducts = orderProducts;

            var user = im.GetCurentUser();
            string userID = user.Id;

            newOrder.UserID = userID;

            return RedirectToAction("Index");
        }
    }
}