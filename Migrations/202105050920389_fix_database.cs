namespace MVCShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProductsPerPage", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Visible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Visible");
            DropColumn("dbo.AspNetUsers", "ProductsPerPage");
        }
    }
}
