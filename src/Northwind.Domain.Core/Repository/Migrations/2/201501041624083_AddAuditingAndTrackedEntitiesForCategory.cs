namespace Northwind.Domain.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddAuditingAndTrackedEntitiesForCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditPropertyTrails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityId = c.Int(nullable: false),
                        EntityType = c.String(nullable: false, maxLength: 128),
                        PropertyName = c.String(nullable: false, maxLength: 128),
                        ModifiedBy = c.String(nullable: false, maxLength: 128),
                        OldValue = c.String(nullable: false, maxLength: 128),
                        NewValue = c.String(nullable: false, maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Categories", "CreatedBy", c => c.String());
            AddColumn("dbo.Categories", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Categories", "ModifiedBy", c => c.String());
            AddColumn("dbo.Categories", "ModifiedOn", c => c.DateTime(nullable: false));

            Sql("UPDATE dbo.Categories SET CreatedBy = 'Migration', CreatedOn = '2015-1-1', ModifiedBy = 'Migration', ModifiedOn = '2015-1-1'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ModifiedOn");
            DropColumn("dbo.Categories", "ModifiedBy");
            DropColumn("dbo.Categories", "CreatedOn");
            DropColumn("dbo.Categories", "CreatedBy");
            DropTable("dbo.AuditPropertyTrails");
        }
    }
}
