using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCShop.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Display(Name="Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Widoczna")]
        public bool Visible { get; set; }

        [Display(Name = "Kategoria nadrzędna")]
        [ForeignKey("CategoryType")]
        public int? CategoryTypeID { get; set; }
        public virtual Category CategoryType { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}