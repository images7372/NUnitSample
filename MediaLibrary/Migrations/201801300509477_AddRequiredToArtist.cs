namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToArtist : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artists", "Name", c => c.String());
        }
    }
}
