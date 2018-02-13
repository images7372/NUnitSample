namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTitleToTrack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "Title");
        }
    }
}
