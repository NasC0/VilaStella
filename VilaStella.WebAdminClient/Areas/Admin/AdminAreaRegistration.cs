using System.Web.Mvc;

namespace VilaStella.WebAdminClient.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Unauthorized access",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login" });

            context.MapRoute(
                name: "Index pagination",
                url: "Admin/Reservations/Index/{page}",
                defaults: new { controller = "Reservations", action = "Index", page = UrlParameter.Optional }); 

            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Reservations", action = "New", id = UrlParameter.Optional }); 
        }
    }
}