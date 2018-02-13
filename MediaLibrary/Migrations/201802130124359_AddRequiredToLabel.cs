namespace MediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToLabel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Labels", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Labels", "Name", c => c.String());
        }
    }
}
