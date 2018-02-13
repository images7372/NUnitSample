namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptimizeAllCardinality : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Artists", "Recording_Id", "dbo.Recordings");
            DropForeignKey("dbo.Labels", "Recording_Id", "dbo.Recordings");
            DropForeignKey("dbo.Artists", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Genres", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.Recordings", "Track_Id", "dbo.Tracks");
            DropIndex("dbo.Artists", new[] { "Recording_Id" });
            DropIndex("dbo.Artists", new[] { "Track_Id" });
            DropIndex("dbo.Genres", new[] { "Track_Id" });
            DropIndex("dbo.Labels", new[] { "Recording_Id" });
            DropIndex("dbo.Recordings", new[] { "Track_Id" });
            AddColumn("dbo.Recordings", "Label_Id", c => c.Int());
            AddColumn("dbo.Recordings", "Artist_Id", c => c.Int());
            AddColumn("dbo.Tracks", "Artist_Id", c => c.Int());
            AddColumn("dbo.Tracks", "Genre_Id", c => c.Int());
            AddColumn("dbo.Tracks", "Recording_Id", c => c.Int());
            CreateIndex("dbo.Recordings", "Label_Id");
            CreateIndex("dbo.Recordings", "Artist_Id");
            CreateIndex("dbo.Tracks", "Artist_Id");
            CreateIndex("dbo.Tracks", "Genre_Id");
            CreateIndex("dbo.Tracks", "Recording_Id");
            AddForeignKey("dbo.Recordings", "Label_Id", "dbo.Labels", "Id");
            AddForeignKey("dbo.Tracks", "Artist_Id", "dbo.Artists", "Id");
            AddForeignKey("dbo.Tracks", "Genre_Id", "dbo.Genres", "Id");
            AddForeignKey("dbo.Tracks", "Recording_Id", "dbo.Recordings", "Id");
            AddForeignKey("dbo.Recordings", "Artist_Id", "dbo.Artists", "Id");
            DropColumn("dbo.Artists", "Recording_Id");
            DropColumn("dbo.Artists", "Track_Id");
            DropColumn("dbo.Genres", "Track_Id");
            DropColumn("dbo.Labels", "Recording_Id");
            DropColumn("dbo.Recordings", "LabelId");
            DropColumn("dbo.Recordings", "ArtistId");
            DropColumn("dbo.Recordings", "ReviewId");
            DropColumn("dbo.Recordings", "Track_Id");
            DropColumn("dbo.Tracks", "GenreId");
            DropColumn("dbo.Tracks", "ArtistId");
            DropColumn("dbo.Tracks", "RecordingId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tracks", "RecordingId", c => c.Int(nullable: false));
            AddColumn("dbo.Tracks", "ArtistId", c => c.Int(nullable: false));
            AddColumn("dbo.Tracks", "GenreId", c => c.Int(nullable: false));
            AddColumn("dbo.Recordings", "Track_Id", c => c.Int());
            AddColumn("dbo.Recordings", "ReviewId", c => c.Int(nullable: false));
            AddColumn("dbo.Recordings", "ArtistId", c => c.Int(nullable: false));
            AddColumn("dbo.Recordings", "LabelId", c => c.Int(nullable: false));
            AddColumn("dbo.Labels", "Recording_Id", c => c.Int());
            AddColumn("dbo.Genres", "Track_Id", c => c.Int());
            AddColumn("dbo.Artists", "Track_Id", c => c.Int());
            AddColumn("dbo.Artists", "Recording_Id", c => c.Int());
            DropForeignKey("dbo.Recordings", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.Tracks", "Recording_Id", "dbo.Recordings");
            DropForeignKey("dbo.Tracks", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.Tracks", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.Recordings", "Label_Id", "dbo.Labels");
            DropIndex("dbo.Tracks", new[] { "Recording_Id" });
            DropIndex("dbo.Tracks", new[] { "Genre_Id" });
            DropIndex("dbo.Tracks", new[] { "Artist_Id" });
            DropIndex("dbo.Recordings", new[] { "Artist_Id" });
            DropIndex("dbo.Recordings", new[] { "Label_Id" });
            DropColumn("dbo.Tracks", "Recording_Id");
            DropColumn("dbo.Tracks", "Genre_Id");
            DropColumn("dbo.Tracks", "Artist_Id");
            DropColumn("dbo.Recordings", "Artist_Id");
            DropColumn("dbo.Recordings", "Label_Id");
            CreateIndex("dbo.Recordings", "Track_Id");
            CreateIndex("dbo.Labels", "Recording_Id");
            CreateIndex("dbo.Genres", "Track_Id");
            CreateIndex("dbo.Artists", "Track_Id");
            CreateIndex("dbo.Artists", "Recording_Id");
            AddForeignKey("dbo.Recordings", "Track_Id", "dbo.Tracks", "Id");
            AddForeignKey("dbo.Genres", "Track_Id", "dbo.Tracks", "Id");
            AddForeignKey("dbo.Artists", "Track_Id", "dbo.Tracks", "Id");
            AddForeignKey("dbo.Labels", "Recording_Id", "dbo.Recordings", "Id");
            AddForeignKey("dbo.Artists", "Recording_Id", "dbo.Recordings", "Id");
        }
    }
}
