namespace Common_Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuItemEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubItemEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MenuItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuItemEntities", t => t.MenuItemId, cascadeDelete: true)
                .Index(t => t.MenuItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubItemEntities", "MenuItemId", "dbo.MenuItemEntities");
            DropIndex("dbo.SubItemEntities", new[] { "MenuItemId" });
            DropTable("dbo.SubItemEntities");
            DropTable("dbo.MenuItemEntities");
        }
    }
}
