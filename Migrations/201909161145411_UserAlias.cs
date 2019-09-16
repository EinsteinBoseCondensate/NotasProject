namespace NotasProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAlias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Alias", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Alias");
        }
    }
}
