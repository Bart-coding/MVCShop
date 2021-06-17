using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCShop.DTO
{
    public class OrderProductsDto
    {
        [Required]
        [Display(Name = "Metoda płatności")]
        public string PaymentMethod { get; set; }

        [Required]
        [Display(Name = "Sposób wysyłki")]
        public string ShippingMethod { get; set; }

    }
}