namespace ZavenDotNetInterview.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFailureCounter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "FailureCounter", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "FailureCounter");
        }
    }
}
