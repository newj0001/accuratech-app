namespace Common_Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        MenuItemId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.SubItemEntities", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubItemEntities", "Value", c => c.String());
            DropTable("dbo.Registrations");
        }
    }
}
