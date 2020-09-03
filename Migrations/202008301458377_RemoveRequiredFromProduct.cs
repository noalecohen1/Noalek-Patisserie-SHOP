namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BranchID);
            
            AlterColumn("dbo.Products", "Name", c => c.String());
            AlterColumn("dbo.Products", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ImagePath", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            DropTable("dbo.Branches");
        }
    }
}
