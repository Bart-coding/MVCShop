using System.Collections.Generic;

namespace MVCShop.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string State { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; }
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}