namespace Patisserie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        IsModerator = c.Boolean(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Address = c.String(),
                        PhoneNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccountID, t.ProductID })
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.AccountID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImagePath = c.String(),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        IsDairy = c.Boolean(nullable: false),
                        IsGlutenFree = c.Boolean(nullable: false),
                        IsVegan = c.Boolean(nullable: false),
                        PopularityRate = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImagePath = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.OrdersHistories",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        OrderNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccountID, t.ProductID })
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.AccountID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrdersHistories", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrdersHistories", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Orders", "AccountID", "dbo.Accounts");
            DropIndex("dbo.OrdersHistories", new[] { "ProductID" });
            DropIndex("dbo.OrdersHistories", new[] { "AccountID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.Orders", new[] { "ProductID" });
            DropIndex("dbo.Orders", new[] { "AccountID" });
            DropTable("dbo.OrdersHistories");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.Accounts");
        }
    }
}
