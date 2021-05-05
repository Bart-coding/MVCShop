using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCShop.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Field {0} can't be empty")]
        [StringLength(30, ErrorMessage = "{0} can be max {1} characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} can't be empty")]
        public decimal Price { get; set; }

        public string Descritpion { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        [Range(0, 100,ErrorMessage = "{0} must be between {1} and {2}")]
        public int Discount { get; set; }

        [Required(ErrorMessage = "Field {0} can't be empty")]
        [Range(-1, 100, ErrorMessage = "{0} must be between {1} and {2}")]
        public int VAT { get; set; }

        public bool Deleted { get; set; }

        [Required(ErrorMessage = "Field {0} can't be empty")]
        public int Quantity { get; set; }

        public int SalesCounter { get; set; }

        public bool Visible { get; set; }

        [Required(ErrorMessage = "Field {0} can't be empty")]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}