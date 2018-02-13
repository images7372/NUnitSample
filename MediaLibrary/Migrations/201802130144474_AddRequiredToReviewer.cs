namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToReviewer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reviewers", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviewers", "Name", c => c.String());
        }
    }
}
