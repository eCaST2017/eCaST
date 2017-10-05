using System.Web;
using System.Web.Optimization;

namespace CTL
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerycustom").Include(
                       "~/Scripts/select2.js",
                        "~/Scripts/moment.min.js",
                       "~/Scripts/jquery.backstretch.min.js",
                        "~/Scripts/jQueryRotate.js",
                        "~/Scripts/jasny-bootstrap.js",
                        //"~/Scripts/dataTables.bootstrap.js",
                        "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js",
                         "~/Scripts/plugins/metisMenu/metisMenu.min.js",
                        "~/Scripts/sb-admin-2.js",
                        "~/Scripts/kendo/2013.1.319/kendo.all.min.js",
                        "~/Scripts/app/app.js",
                        //"~/Scripts/jquery.mask.js",
                         "~/Scripts/jquery.maskedinput.js",
                       "~/Scripts/kendo/2013.1.319/kendo.dataviz.min.js",
                      "~/Scripts/kendo/2013.1.319/kendo.aspnetmvc.min.js",
                     "~/Scripts/kendo/2013.1.319/kendo.web.min.js",
                    "~/Scripts/kendo/2013.1.319/kendo.editor.min.js",
                    "~/Scripts/kendo/2013.1.319/kendo.editable.min.js",
                     "~/Scripts/toastr.js",
                     "~/Scripts/redactor/redactor.js",
                      "~/Scripts/imagemanager.js",
                      "~/Scripts/dropzone.js",
                     "~/Scripts/bootstrap-fileupload.js",
                     "~/Scripts/tableau_v8.js"
                     //"~/Scripts/wysihtml5-0.3.0.js"
                     
                     ));


      
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                  "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                       //"~/Content/bootstrap-theme.css",
                      "~/Content/site.css", 
                      "~/Content/select2.css",
                      //"~/Content/jasny-bootstrap.css",
                       //"~/Content/dataTables.bootstrap.css",
                      "~/Content/kendo/2013.1.319/kendo.default.min.css",
                      "~/Content/DataTables-1.9.4/media/css/jquery.dataTables.css",
                    "~/Content/kendo/2013.1.319/kendo.common.min.css",
                    "~/Content/plugins/metisMenu/metisMenu.min.css",
                     "~/Content/sb-admin-2.css",
                     "~/font-awesome-4.1.0/css/font-awesome.min.css",
                   "~/Content/kendo/2013.1.319/kendo.default.min.css",
                  "~/Content/kendo/2013.1.319/kendo.dataviz.min.css",
                  "~/Content/toastr.css",
                   "~/Content/w3.css",
                   "~/Content/redactor/redactor.css",
                  "~/Content/dropzone.css"
                  //"~/Content/main.css"
                  // "~/Content/jHtmlArea/jHtmlAreaColorPickerMenu.css"
                   ));


          
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
