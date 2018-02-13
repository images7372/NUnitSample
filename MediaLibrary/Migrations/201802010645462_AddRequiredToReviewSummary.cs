namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToReviewSummary : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reviews", "Summary", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Summary", c => c.String());
        }
    }
}
