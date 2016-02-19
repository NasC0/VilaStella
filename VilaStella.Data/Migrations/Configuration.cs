namespace VilaStella.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using VilaStella.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DefaultDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;

            //TODO: Remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DefaultDbContext context)
        {
            SetDefaultSettings(context);
        }

        private void SetDefaultSettings(DefaultDbContext context)
        {
            if (context.GeneralSettings.Count() == 0)
            {
                decimal defaultPricing = 100.00m;
                bool areReservationsOpen = true;

                var settings = new GeneralSettings()
                {
                    Pricing = defaultPricing,
                    AreReservationsOpen = areReservationsOpen
                };

                context.GeneralSettings.Add(settings);
                context.SaveChanges();
            }
        }
    }
}
