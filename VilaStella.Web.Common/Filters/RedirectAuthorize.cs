using System.Web.Mvc;

namespace VilaStella.Web.Common.Filters
{
    public class RedirectAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(this.Redirect);
        }

        public string Redirect { get; set; }
    }
}
