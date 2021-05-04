using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCShop.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }

        [ForeignKey("CategoryType")]
        public int? CategoryTypeID { get; set; }
        public virtual Category CategoryType { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}