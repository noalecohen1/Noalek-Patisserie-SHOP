namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeOfPhoneInAccountModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "PhoneNumber", c => c.Int(nullable: false));
        }
    }
}
