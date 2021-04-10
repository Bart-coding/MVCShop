using MVCShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MyDBContext context = new MyDBContext();
            CategoryType categoryType = new CategoryType()
            {
                Name = "spożywcze"
            };
            context.CategoryTypes.Add(categoryType);
            context.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}