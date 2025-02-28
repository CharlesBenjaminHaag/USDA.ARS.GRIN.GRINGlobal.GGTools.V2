using System.Web;
using System.Web.Optimization;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/bower_components/jquery/dist/jquery.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
             "~/Content/dist/js/adminlte.js"));

            bundles.Add(new ScriptBundle("~/ggtools/js").Include(
             "~/Content/dist/js/ggtools.js"));

            bundles.Add(new ScriptBundle("~/ckeditor/js").Include(
             "~/Content/dist/js/ggtools.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/dist/css/AdminLTE.css",
                      "~/Content/site.css"));
        }
    }
}
