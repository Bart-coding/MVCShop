namespace MVCShop.Models
{
    public class OrderProduct
    {
        public int OrderProductID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int NumberOfProducts { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}