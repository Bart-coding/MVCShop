using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace MVCShop.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Netto { get; set; }
        public bool Newsletter { get; set; }
        public int ProductsPerPage { get; set; }
        public int PersonalDiscount { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
    }

	public class IdentityManager
	{
		public RoleManager<IdentityRole> LocalRoleManager
		{
			get
			{
				return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
			}
		}

		public UserManager<ApplicationUser> LocalUserManager
		{
			get
			{
				return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
			}
		}

		public ApplicationUser GetUserByID(string userID)
		{
			UserManager<ApplicationUser> um = this.LocalUserManager;
			ApplicationUser user = um.FindById(userID);

			return user;
		}

		public ApplicationUser GetUserByEmail(string email)
		{
			UserManager<ApplicationUser> um = this.LocalUserManager;
			ApplicationUser user = um.FindByEmail(email);

			return user;
		}

		public ApplicationUser GetCurentUser()
		{
			string userID = HttpContext.Current.User.Identity.GetUserId();

			return GetUserByID(userID);
		}

		private bool RoleExists(string name)
		{
			var rm = LocalRoleManager;
			return rm.RoleExists(name);
		}

		public bool CreateRole(string name)
		{
			var rm = LocalRoleManager;
			if (!RoleExists(name))
            {
				var idResult = rm.Create(new IdentityRole(name));
				return idResult.Succeeded;
			}
			return false;
		}

		public bool AddUserToRoleById(string userId, string roleName)
		{
			var um = LocalUserManager;
			var idResult = um.AddToRole(userId, roleName);

			return idResult.Succeeded;
		}

		public bool AddUserToRoleByUsername(string username, string roleName)
		{
			var um = LocalUserManager;
			string userID = um.FindByName(username).Id;
			var idResult = um.AddToRole(userID, roleName);

			return idResult.Succeeded;
		}

		public bool AddCurrentUserToRole(string roleName)
		{
			var um = LocalUserManager;
			string userID = HttpContext.Current.User.Identity.GetUserId();
			var idResult = um.AddToRole(userID, roleName);

			return idResult.Succeeded;
		}

		public void ClearUserRoles(string userId)
		{
			var um = LocalUserManager;
			var user = um.FindById(userId);
			var currentRoles = new List<IdentityUserRole>();

			currentRoles.AddRange(user.Roles);

			foreach (var role in currentRoles)
			{
				um.RemoveFromRole(userId, role.RoleId);
			}
		}
	}
}