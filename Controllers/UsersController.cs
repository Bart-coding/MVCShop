using Microsoft.AspNet.Identity.Owin;
using MVCShop.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    [Authorize(Roles ="admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private IdentityManager im = new IdentityManager();

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = UserManager.Users;
            return View(await users.ToListAsync());
        }

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userToEdit = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.Name,
                Surname = user.Surname,
                ProductsPerPage = user.ProductsPerPage,
                PersonalDiscount = user.PersonalDiscount,
                Netto = user.Netto,
                Newsletter = user.Newsletter,
                PostalCode = user.Address.PostalCode,
                City = user.Address.City,
                StreetAddress = user.Address.StreetAddress
            };
            return View(userToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByIdAsync(model.Id).Result;
                
                user.ProductsPerPage = model.ProductsPerPage;
                user.PersonalDiscount = model.PersonalDiscount;
                user.Newsletter = model.Newsletter;
                user.Netto = model.Netto;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // jeśli chcielibyśmy móc edytować również adres
                    //var address = await db.Addresses.FirstOrDefaultAsync(x => x.UserID == model.Id);

                    //address.PostalCode = model.PostalCode;
                    //address.City = model.City;
                    //address.StreetAddress = model.StreetAddress;

                    //db.Entry(address).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            // wyczyszczenie wszytkich ról użytkownika
            im.ClearUserRoles(id);
            
            // usunięcie adresu użytkownika z bazy
            var address = await db.Addresses.FirstOrDefaultAsync(x => x.UserID == id);
            db.Addresses.Remove(address);
            await db.SaveChangesAsync();

            // usunięcie samego użytkownika
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            await UserManager.DeleteAsync(user);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}