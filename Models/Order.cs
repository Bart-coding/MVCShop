using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCShop.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [Display(Name = "Stan")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Metoda płatności")]
        public string PaymentMethod { get; set; }

        [Required]
        [Display(Name = "Sposób wysyłki")]
        public string ShippingMethod { get; set; }

        [Required]
        [Display(Name = "Koszt zamówienia")]
        public decimal Cost { get; set; }

        [Required]
        [Display(Name = "Klient")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Produkty")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }
}