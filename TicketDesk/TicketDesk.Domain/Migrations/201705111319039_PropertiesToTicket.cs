namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertiesToTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "WorkingHours", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "WorkingHours");
        }
    }
}
