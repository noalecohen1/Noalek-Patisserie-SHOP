namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredToCategory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Categories", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "ImagePath", c => c.String());
            AlterColumn("dbo.Categories", "Name", c => c.String());
        }
    }
}
