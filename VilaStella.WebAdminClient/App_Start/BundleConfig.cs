using System.Web;
using System.Web.Optimization;

namespace VilaStella.WebAdminClient
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.ui.datepicker-bg.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/_extensions.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/jquery.easing.min.js",
                "~/Scripts/jquery.backstretch.min.js",
                "~/Scripts/App/Root/grayscale.js",
                "~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/jquery.easing.min.js",
                "~/Scripts/toastr.js",
                "~/Scripts/App/Admin/admin.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Admin/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Areas/Admin/Content/site.css",
                      "~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/Styles/jqueryUI").Include(
                "~/Content/themes/base/datepicker.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            // TODO: Set to true in production
            BundleTable.EnableOptimizations = false;
        }
    }
}
