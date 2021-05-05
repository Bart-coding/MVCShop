namespace MVCShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_annotations_to_product : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
