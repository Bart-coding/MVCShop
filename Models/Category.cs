using System.Collections.Generic;

namespace MVCShop.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public int CategoryTypeID { get; set; }
        public virtual CategoryType CategoryType { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}