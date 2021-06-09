using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCShop.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [Display(Name = "Stan")]
        public string State { get; set; }

        [Display(Name = "Metoda płatności")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Sposób wysyłki")]
        public string ShippingMethod { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Produkty")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}