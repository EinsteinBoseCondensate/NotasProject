namespace NotasProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNota : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notas", "Nonce", c => c.String());
            DropColumn("dbo.Notas", "IsPublic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notas", "IsPublic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Notas", "Nonce");
        }
    }
}
