namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _added_OnlineSupport_column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "OnlineSupport", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "OnlineSupport");
        }
    }
}
