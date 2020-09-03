namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnnotationInOrdersHistoryModel : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OrdersHistories");
            AddPrimaryKey("dbo.OrdersHistories", new[] { "AccountID", "ProductID", "OrderNumber" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OrdersHistories");
            AddPrimaryKey("dbo.OrdersHistories", new[] { "AccountID", "ProductID" });
        }
    }
}
