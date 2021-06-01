using MVCShop.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        // tak stworzono role
        //public string Create()
        //{
        //    IdentityManager im = new IdentityManager();

        //    im.CreateRole("admin");
        //    im.CreateRole("user");

        //    return "OK";
        //}

        // jakbyśmy chcieli dodać drugiego admina czy coś
        //public string AddCurrentUserToAdminRole()
        //{
        //    IdentityManager im = new IdentityManager();

        //    var result = im.AddCurrentUserToRole("admin");

        //    return result.ToString();
        //}

        public ActionResult AddCurrentUserToUserRole()
        {
            IdentityManager im = new IdentityManager();
            var result = im.AddCurrentUserToRole("user");
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}