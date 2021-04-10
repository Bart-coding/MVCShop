namespace MVCShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        PostalCode = c.String(),
                        City = c.String(),
                        StreetAddress = c.String(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        Netto = c.Boolean(nullable: false),
                        Newsletter = c.Boolean(nullable: false),
                        PersonalDiscount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        State = c.String(),
                        PaymentMethod = c.String(),
                        ShippingMethod = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        OrderProductID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderProductID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descritpion = c.String(),
                        Picture = c.Binary(),
                        Date = c.DateTime(nullable: false),
                        Discount = c.Int(nullable: false),
                        VAT = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                        SalesCounter = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Visible = c.Boolean(nullable: false),
                        CategoryTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.CategoryTypes", t => t.CategoryTypeID, cascadeDelete: true)
                .Index(t => t.CategoryTypeID);
            
            CreateTable(
                "dbo.CategoryTypes",
                c => new
                    {
                        CategoryTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryTypeID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Content = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "UserID", "dbo.Users");
            DropForeignKey("dbo.Orders", "UserID", "dbo.Users");
            DropForeignKey("dbo.OrderProducts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Files", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Categories", "CategoryTypeID", "dbo.CategoryTypes");
            DropForeignKey("dbo.OrderProducts", "OrderID", "dbo.Orders");
            DropIndex("dbo.Files", new[] { "ProductID" });
            DropIndex("dbo.Categories", new[] { "CategoryTypeID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.OrderProducts", new[] { "ProductID" });
            DropIndex("dbo.OrderProducts", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropIndex("dbo.Addresses", new[] { "UserID" });
            DropTable("dbo.Files");
            DropTable("dbo.CategoryTypes");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Orders");
            DropTable("dbo.Users");
            DropTable("dbo.Addresses");
        }
    }
}
