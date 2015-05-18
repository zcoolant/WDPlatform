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
                    "~/assets/vendor/angular-material/angular-material.css",
                    "~/assets/css/app.css")
            );

            bundles.Add(new ScriptBundle("~/js/vendor").Include(
                    "~/assets/vendor/jquery/jquery.js",
                    "~/assets/vendor/bootstrap/dist/js/bootstrap.min.js",
                    "~/assets/vendor/angular/angular.js",
                    "~/assets/vendor/angular-animate/angular-animate.js",
                    "~/assets/vendor/angular-aria/angular-aria.js",
                    "~/assets/vendor/angular-material/angular-material.js")
            );

            bundles.Add(new ScriptBundle("~/js/app")
                .Include("~/assets/vendor/signalr/jquery.signalR.js", "~/assets/vendor/angular-swing/dist/angular-swing.js", "~/app/app.js")
                .IncludeDirectory("~/app/common", "*.js")
                .IncludeDirectory("~/app/controllers", "*.js")
                .IncludeDirectory("~/app/services", "*.js")
                //.IncludeDirectory("~/app/directives", "*.js")
            );

        }
    }
}
