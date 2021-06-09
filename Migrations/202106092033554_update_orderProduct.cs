namespace MVCShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_orderProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "UserID" });
            AddColumn("dbo.OrderProducts", "NumberOfProducts", c => c.Int(nullable: false));
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String(nullable: false));
            AlterColumn("dbo.Addresses", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Addresses", "StreetAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "PaymentMethod", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "ShippingMethod", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "UserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "UserID");
            AddForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "UserID" });
            AlterColumn("dbo.Orders", "UserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Orders", "ShippingMethod", c => c.String());
            AlterColumn("dbo.Orders", "PaymentMethod", c => c.String());
            AlterColumn("dbo.Addresses", "StreetAddress", c => c.String());
            AlterColumn("dbo.Addresses", "City", c => c.String());
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String());
            DropColumn("dbo.OrderProducts", "NumberOfProducts");
            CreateIndex("dbo.Orders", "UserID");
            AddForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}
