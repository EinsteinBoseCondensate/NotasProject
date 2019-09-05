namespace NotasProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingActivityLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogModels",
                c => new
                    {
                        ActivityLogModelId = c.Int(nullable: false, identity: true),
                        LogTime = c.DateTime(nullable: false),
                        Realm = c.String(),
                        Message = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ActivityLogModelId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActivityLogModels");
        }
    }
}
