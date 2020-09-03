namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "Email", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Branches", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Branches", "Address", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Accounts", "Email", unique: true);
            CreateIndex("dbo.Branches", "Name", unique: true);
            CreateIndex("dbo.Branches", "Address", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Branches", new[] { "Address" });
            DropIndex("dbo.Branches", new[] { "Name" });
            DropIndex("dbo.Accounts", new[] { "Email" });
            AlterColumn("dbo.Branches", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Branches", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Accounts", "Email", c => c.String(nullable: false));
        }
    }
}
