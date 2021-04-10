using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCShop.Models
{
    public class Address
    {
        [Key]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public virtual User User { get; set; }
    }
}