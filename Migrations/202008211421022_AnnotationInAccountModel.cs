namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnnotationInAccountModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Accounts", "PhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Accounts", "Address", c => c.String());
        }
    }
}
