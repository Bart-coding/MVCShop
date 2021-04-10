using System.Data.Entity;

namespace MVCShop.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("name=MyCS") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
    }
}