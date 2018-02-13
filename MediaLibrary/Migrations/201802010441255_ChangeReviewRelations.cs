namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeReviewRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviewers", "Review_ID", "dbo.Reviews");
            DropIndex("dbo.Reviewers", new[] { "Review_ID" });
            AddColumn("dbo.Recordings", "ReviewId", c => c.Int(nullable: false));
            AddColumn("dbo.Reviewers", "ReviewId", c => c.Int(nullable: false));
            AddColumn("dbo.Reviews", "Recording_Id", c => c.Int());
            AddColumn("dbo.Reviews", "Reviewer_Id", c => c.Int());
            CreateIndex("dbo.Reviews", "Recording_Id");
            CreateIndex("dbo.Reviews", "Reviewer_Id");
            AddForeignKey("dbo.Reviews", "Recording_Id", "dbo.Recordings", "Id");
            AddForeignKey("dbo.Reviews", "Reviewer_Id", "dbo.Reviewers", "Id");
            DropColumn("dbo.Reviewers", "Review_ID");
            DropColumn("dbo.Reviews", "ReviewerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "ReviewerId", c => c.Int(nullable: false));
            AddColumn("dbo.Reviewers", "Review_ID", c => c.Int());
            DropForeignKey("dbo.Reviews", "Reviewer_Id", "dbo.Reviewers");
            DropForeignKey("dbo.Reviews", "Recording_Id", "dbo.Recordings");
            DropIndex("dbo.Reviews", new[] { "Reviewer_Id" });
            DropIndex("dbo.Reviews", new[] { "Recording_Id" });
            DropColumn("dbo.Reviews", "Reviewer_Id");
            DropColumn("dbo.Reviews", "Recording_Id");
            DropColumn("dbo.Reviewers", "ReviewId");
            DropColumn("dbo.Recordings", "ReviewId");
            CreateIndex("dbo.Reviewers", "Review_ID");
            AddForeignKey("dbo.Reviewers", "Review_ID", "dbo.Reviews", "ID");
        }
    }
}
