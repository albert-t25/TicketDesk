namespace TicketDesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewPropertiesToTicketForRaport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "WorkingDays", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "WithSupport", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tickets", "WithPersonalAuto", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tickets", "WithArfaNetAuto", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "WithArfaNetAuto");
            DropColumn("dbo.Tickets", "WithPersonalAuto");
            DropColumn("dbo.Tickets", "WithSupport");
            DropColumn("dbo.Tickets", "WorkingDays");
        }
    }
}
