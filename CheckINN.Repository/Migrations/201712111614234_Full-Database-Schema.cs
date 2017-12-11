 namespace CheckINN.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FullDatabaseSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        CheckId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ShopId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CheckId)
                .ForeignKey("dbo.Shops", t => t.ShopId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ShopId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        ShopId = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ShopId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Salt = c.Binary(),
                        PasswordHash = c.Binary(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ProductListings",
                c => new
                    {
                        ProductListingId = c.Int(nullable: false, identity: true),
                        CheckId = c.Int(nullable: false),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProductListingId)
                .ForeignKey("dbo.Checks", t => t.CheckId, cascadeDelete: true)
                .Index(t => t.CheckId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Shops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.ProductListings", "CheckId", "dbo.Checks");
            DropForeignKey("dbo.Checks", "UserId", "dbo.Users");
            DropForeignKey("dbo.Checks", "ShopId", "dbo.Shops");
            DropIndex("dbo.Products", new[] { "ShopId" });
            DropIndex("dbo.ProductListings", new[] { "CheckId" });
            DropIndex("dbo.Checks", new[] { "UserId" });
            DropIndex("dbo.Checks", new[] { "ShopId" });
            DropTable("dbo.Products");
            DropTable("dbo.ProductListings");
            DropTable("dbo.Users");
            DropTable("dbo.Shops");
            DropTable("dbo.Checks");
        }
    }
}
