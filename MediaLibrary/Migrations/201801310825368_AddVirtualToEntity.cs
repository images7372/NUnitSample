namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVirtualToEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "Recording_Id", c => c.Int());
            AddColumn("dbo.Artists", "Track_Id", c => c.Int());
            AddColumn("dbo.Genres", "Track_Id", c => c.Int());
            AddColumn("dbo.Labels", "Recording_Id", c => c.Int());
            AddColumn("dbo.Recordings", "Track_Id", c => c.Int());
            AddColumn("dbo.Reviewers", "Review_ID", c => c.Int());
            CreateIndex("dbo.Artists", "Recording_Id");
            CreateIndex("dbo.Artists", "Track_Id");
            CreateIndex("dbo.Genres", "Track_Id");
            CreateIndex("dbo.Labels", "Recording_Id");
            CreateIndex("dbo.Recordings", "Track_Id");
            CreateIndex("dbo.Reviewers", "Review_ID");
            AddForeignKey("dbo.Artists", "Recording_Id", "dbo.Recordings", "Id");
            AddForeignKey("dbo.Labels", "Recording_Id", "dbo.Recordings", "Id");
            AddForeignKey("dbo.Reviewers", "Review_ID", "dbo.Reviews", "ID");
            AddForeignKey("dbo.Artists", "Track_Id", "dbo.Tracks", "Id");
            AddForeignKey("dbo.Genres", "Track_Id", "dbo.Tracks", "Id");
            AddForeignKey("dbo.Recordings", "Track_Id", "dbo.Tracks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recordings", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Genres", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Artists", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Reviewers", "Review_ID", "dbo.Reviews");
            DropForeignKey("dbo.Labels", "Recording_Id", "dbo.Recordings");
            DropForeignKey("dbo.Artists", "Recording_Id", "dbo.Recordings");
            DropIndex("dbo.Reviewers", new[] { "Review_ID" });
            DropIndex("dbo.Recordings", new[] { "Track_Id" });
            DropIndex("dbo.Labels", new[] { "Recording_Id" });
            DropIndex("dbo.Genres", new[] { "Track_Id" });
            DropIndex("dbo.Artists", new[] { "Track_Id" });
            DropIndex("dbo.Artists", new[] { "Recording_Id" });
            DropColumn("dbo.Reviewers", "Review_ID");
            DropColumn("dbo.Recordings", "Track_Id");
            DropColumn("dbo.Labels", "Recording_Id");
            DropColumn("dbo.Genres", "Track_Id");
            DropColumn("dbo.Artists", "Track_Id");
            DropColumn("dbo.Artists", "Recording_Id");
        }
    }
}
