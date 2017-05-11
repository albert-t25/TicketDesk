namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertiesToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "Phone", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "Address", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Address");
            DropColumn("dbo.Projects", "Phone");
            DropColumn("dbo.Projects", "Email");
        }
    }
}
