using System.Web.Optimization;

namespace Crossdock
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            bundles.Add(new Bundle("~/bundles/custom-js")
                .Include("~/Scripts/start-bootstrap.js",
                         "~/Scripts/fontawesome/all.min.js",
                         "~/Scripts/customFormWizard.js",
                         "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/jquery.dataTables.responsive.js",
                         "~/Scripts/DataTables/jquery.dataTables.checkboxes.min.js",
                         "~/Scripts/jquery-ui.js"));

            //bundles.Add(new StyleBundle("~/Content/custom-css").Include(
            //          "~/Content/start-bootstrap.css",
            //          "~/Content/Site.css",
            //          "~/Content/jquery-ui.css"));

            ////scripts para grid mvc
            //bundles.Add(new ScriptBundle("~/bundles/Gridmvc").Include(
            //     "~/Scripts/gridmvc*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/Content/start-bootstrap.css",
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/jquery.dataTables.checkboxes.css",
                 "~/Content/jquery-ui.css", 
                "~/Content/DataTables/css/responsive.dataTables.css",
                            "~/Content/Gridmvc.css"));

        }
    }
}


