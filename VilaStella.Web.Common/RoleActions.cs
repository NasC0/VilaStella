using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VilaStella.Data;
using VilaStella.Models;

namespace VilaStella.Web.Common
{
    public class RoleActions
    {
        private DefaultDbContext dbContext;

        public RoleActions(DefaultDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddUserAndRole()
        {
            var roleStore = new RoleStore<IdentityRole>(this.dbContext);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            IdentityResult roleResult;
            IdentityResult userResult;

            if (!roleManager.RoleExists("Admin"))
            {
                roleResult = roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.dbContext));

            var appUser = new ApplicationUser
            {
                UserName = "Admin@vilastella.com",
                Email = "Admin@vilastella.com"
            };

            userResult = userManager.Create(appUser, "314159qqww");

            if (!userManager.IsInRole(userManager.FindByEmail("Admin@vilastella.com").Id, "Admin"))
            {
                userResult = userManager.AddToRole(userManager.FindByEmail("Admin@vilastella.com").Id, "Admin");
            }
        }
    }
}
