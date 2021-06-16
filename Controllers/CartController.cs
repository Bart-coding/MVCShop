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

        // GET: Cart
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (Session["cart"] == null)
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
                priceSum += (double)(tempProduct.Price * tempQuantity);

            }

            ViewBag.productsQuantities = productsQuantities;
            ViewBag.priceSum = priceSum;

            return View(listToShow);
        }

        public ActionResult Add(int? productId)
        {
            System.Diagnostics.Debug.WriteLine(productId==null?"siema":"A");
            if (productId != null)
            {
                List<ProductsCountDto> cartProducts;
                if (Session["cart"] == null)
                {
                    cartProducts = new List<ProductsCountDto>();
                }

                else
                {
                    cartProducts = (List<ProductsCountDto>)Session["cart"];
                    int productIndex = cartProducts.FindIndex(p => p.ProductID == productId);
                    if (productIndex > -1)
                    {
                        var product = cartProducts[productIndex];
                        product.Count++;
                        cartProducts[productIndex] = product;
                        return RedirectToAction("Index");
                    }

                }

                cartProducts.Add(new ProductsCountDto { ProductID = (int)productId, Count = 1 });

                Session["cart"] = cartProducts;
            }

            return RedirectToAction("Index");

        }

        public ActionResult Delete(int? productId)
        {
            if (productId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            List<ProductsCountDto> cartProducts;
            if (Session["cart"] == null)
                return View();//

            cartProducts = (List<ProductsCountDto>)Session["cart"];

            int productIndex = cartProducts.FindIndex(p => p.ProductID == productId);
            if (productIndex > -1)
            {
                var product = cartProducts[productIndex];
                if (product.Count > 1)
                {
                    product.Count--;
                    cartProducts[productIndex] = product;
                }
                else
                    cartProducts.RemoveAt(productIndex);

            }

            Session["cart"] = cartProducts;

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
    }
}