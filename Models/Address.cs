using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCShop.Models
{
    public class Address
    {
        [Key]
        [ForeignKey("User")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        public string StreetAddress { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}