namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToRecordingTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recordings", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recordings", "Title", c => c.String());
        }
    }
}
