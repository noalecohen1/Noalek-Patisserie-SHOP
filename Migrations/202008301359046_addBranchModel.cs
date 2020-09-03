namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBranchModel : DbMigration
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
                    PhoneNumber = c.String(nullable: false)
                })
                .PrimaryKey(t => new { t.BranchID });
        }
        
        public override void Down()
        {
            DropTable("dbo.Branches");
        }
    }
}
