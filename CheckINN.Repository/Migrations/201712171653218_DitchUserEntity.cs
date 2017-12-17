namespace CheckINN.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DitchUserEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Checks", "UserId", "dbo.Users");
            DropIndex("dbo.Checks", new[] { "UserId" });
            DropColumn("dbo.Checks", "UserId");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.Checks", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Checks", "UserId");
            AddForeignKey("dbo.Checks", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
