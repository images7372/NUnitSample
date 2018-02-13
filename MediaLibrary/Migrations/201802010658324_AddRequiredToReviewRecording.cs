namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToReviewRecording : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "Recording_Id", "dbo.Recordings");
            DropIndex("dbo.Reviews", new[] { "Recording_Id" });
            AlterColumn("dbo.Reviews", "Recording_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "Recording_Id");
            AddForeignKey("dbo.Reviews", "Recording_Id", "dbo.Recordings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Recording_Id", "dbo.Recordings");
            DropIndex("dbo.Reviews", new[] { "Recording_Id" });
            AlterColumn("dbo.Reviews", "Recording_Id", c => c.Int());
            CreateIndex("dbo.Reviews", "Recording_Id");
            AddForeignKey("dbo.Reviews", "Recording_Id", "dbo.Recordings", "Id");
        }
    }
}
