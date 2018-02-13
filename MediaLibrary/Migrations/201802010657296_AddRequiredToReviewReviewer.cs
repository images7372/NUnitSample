namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToReviewReviewer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "Reviewer_Id", "dbo.Reviewers");
            DropIndex("dbo.Reviews", new[] { "Reviewer_Id" });
            AlterColumn("dbo.Reviews", "Reviewer_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "Reviewer_Id");
            AddForeignKey("dbo.Reviews", "Reviewer_Id", "dbo.Reviewers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Reviewer_Id", "dbo.Reviewers");
            DropIndex("dbo.Reviews", new[] { "Reviewer_Id" });
            AlterColumn("dbo.Reviews", "Reviewer_Id", c => c.Int());
            CreateIndex("dbo.Reviews", "Reviewer_Id");
            AddForeignKey("dbo.Reviews", "Reviewer_Id", "dbo.Reviewers", "Id");
        }
    }
}
