namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteInvalidAttributeFromReviewer : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reviewers", "ReviewId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviewers", "ReviewId", c => c.Int(nullable: false));
        }
    }
}
