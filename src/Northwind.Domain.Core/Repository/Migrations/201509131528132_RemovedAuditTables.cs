namespace Northwind.Domain.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedAuditTables : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AuditPropertyTrails");
        }
        
        public override void Down()
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
        }
    }
}
