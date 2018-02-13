namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToTrack : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tracks", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tracks", "Title", c => c.String());
        }
    }
}
