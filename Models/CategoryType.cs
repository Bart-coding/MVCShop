using System.Collections.Generic;

namespace MVCShop.Models
{
    public class CategoryType
    {
        public int CategoryTypeID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}