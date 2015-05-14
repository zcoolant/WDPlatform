using System.Web;
using System.Web.Optimization;

namespace WDPlatform
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css/main").Include(
                    "~/assets/vendor/bootstrap/dist/css/bootstrap.css",
                    "~/assets/css/jquery-ui.css",
                    "~/assets/css/app.css")
            );

            bundles.Add(new ScriptBundle("~/js/vendor").Include(
                    "~/assets/vendor/jquery/jquery.js",
                    "~/assets/vendor/bootstrap/dist/js/bootstrap.min.js",
                    "~/assets/vendor/angular/angular.js",
                    "~/app/common/jquery-ui-1.9.2.min.js")
            );


            bundles.Add(new ScriptBundle("~/js/app")
                .Include("~/assets/vendor/signalr/jquery.signalR.js", "~/app/app.js","~/app/directives/angular-dragdrop.min.js")
                .IncludeDirectory("~/app/common", "*.js")
                .IncludeDirectory("~/app/controllers", "*.js")
                .IncludeDirectory("~/app/services", "*.js")
                .IncludeDirectory("~/app/directives", "*.js")
            );

        }
    }
}
