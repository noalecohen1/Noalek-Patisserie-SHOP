namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredToProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, defaultValue: ""));
            AlterColumn("dbo.Products", "ImagePath", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ImagePath", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
