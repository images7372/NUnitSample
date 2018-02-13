namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToRecordingLabel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recordings", "Label_Id", "dbo.Labels");
            DropIndex("dbo.Recordings", new[] { "Label_Id" });
            AlterColumn("dbo.Recordings", "Label_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Recordings", "Label_Id");
            AddForeignKey("dbo.Recordings", "Label_Id", "dbo.Labels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recordings", "Label_Id", "dbo.Labels");
            DropIndex("dbo.Recordings", new[] { "Label_Id" });
            AlterColumn("dbo.Recordings", "Label_Id", c => c.Int());
            CreateIndex("dbo.Recordings", "Label_Id");
            AddForeignKey("dbo.Recordings", "Label_Id", "dbo.Labels", "Id");
        }
    }
}
