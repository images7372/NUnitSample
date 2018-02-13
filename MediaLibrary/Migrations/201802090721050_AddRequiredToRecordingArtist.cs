namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToRecordingArtist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recordings", "Artist_Id", "dbo.Artists");
            DropIndex("dbo.Recordings", new[] { "Artist_Id" });
            AlterColumn("dbo.Recordings", "Artist_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Recordings", "Artist_Id");
            AddForeignKey("dbo.Recordings", "Artist_Id", "dbo.Artists", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recordings", "Artist_Id", "dbo.Artists");
            DropIndex("dbo.Recordings", new[] { "Artist_Id" });
            AlterColumn("dbo.Recordings", "Artist_Id", c => c.Int());
            CreateIndex("dbo.Recordings", "Artist_Id");
            AddForeignKey("dbo.Recordings", "Artist_Id", "dbo.Artists", "Id");
        }
    }
}
