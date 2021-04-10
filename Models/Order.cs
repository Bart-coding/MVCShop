using System.Collections.Generic;

namespace MVCShop.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string State { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}