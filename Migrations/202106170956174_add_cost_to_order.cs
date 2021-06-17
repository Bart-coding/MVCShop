namespace MVCShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_cost_to_order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Cost");
        }
    }
}
