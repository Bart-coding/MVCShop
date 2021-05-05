using System;
using System.Collections.Generic;

namespace MVCShop.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Descritpion { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Date { get; set; }
        public int Discount { get; set; }
        public int VAT { get; set; }
        public bool Deleted { get; set; }
        public int Quantity { get; set; }
        public int SalesCounter { get; set; }
        public bool Visible { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}