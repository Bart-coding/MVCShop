using System.Collections.Generic;

namespace MVCShop.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Netto { get; set; }
        public bool Newsletter { get; set; }
        public int PersonalDiscount { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}