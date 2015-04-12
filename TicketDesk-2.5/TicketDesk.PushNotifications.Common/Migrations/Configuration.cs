using System.Configuration;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.PushNotifications.Common.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<TdPushNotificationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicketDeskPushNotifications";
        }

        protected override void Seed(TicketDesk.PushNotifications.Common.TdPushNotificationContext context)
        {
            var demoMode = ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"];
            if (!string.IsNullOrEmpty(demoMode) && demoMode.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                DemoPushNotificationDataManager.SetupDemoPushNotificationData(context);
            }
            else
            {
                if (!context.SubscriberPushNotificationSettings.Any(s => s.SubscriberId == "64165817-9cb5-472f-8bfb-6a35ca54be6a"))
                {
                    context.SubscriberPushNotificationSettings.Add(new SubscriberPushNotificationSetting()
                    {
                        SubscriberId = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                        IsEnabled = true,
                        PushNotificationDestinations = new PushNotificationDestinationCollection()
                        {
                            new PushNotificationDestination()
                            {
                                SubscriberName = "Admin User",
                                DestinationAddress = "admin@example.com",
                                DestinationType = "email"
                            }
                        }
                    });
                }
            }
            base.Seed(context);
        }
    }
}