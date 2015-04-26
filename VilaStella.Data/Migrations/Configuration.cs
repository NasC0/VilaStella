namespace VilaStella.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using VilaStella.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DefaultDbContext>
    {
        private const string ADMIN = "******";
        private const string ADMIN_PASSWORD = "******";
        private const string ADMIN_ROLE = "Admin";
        private const string TEST = "TestTest";

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;

            //TODO: Remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DefaultDbContext context)
        {
            InitializeIdentityForEF(context);
            SetDefaultSettings(context);
        }

        private void InitializeIdentityForEF(DefaultDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //Create Role Test and User Test
            RoleManager.Create(new IdentityRole(TEST));
            UserManager.Create(new ApplicationUser() { UserName = TEST }, TEST);

            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(ADMIN_ROLE))
            {
                var roleresult = RoleManager.Create(new IdentityRole(ADMIN_ROLE));
            }

            //Create User=Admin with password=123456
            var user = new ApplicationUser();
            user.UserName = ADMIN;
            var adminresult = UserManager.Create(user, ADMIN_PASSWORD);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, ADMIN_ROLE);
            }
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
