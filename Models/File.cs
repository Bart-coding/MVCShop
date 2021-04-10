namespace MVCShop.Models
{
    public class File
    {
        public int FileID { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}