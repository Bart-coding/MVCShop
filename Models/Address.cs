using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCShop.Models
{
    public class Address
    {
        [Key]
        [ForeignKey("User")]
        public string UserID { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}