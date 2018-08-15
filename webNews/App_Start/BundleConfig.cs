using System.Web;
using System.Web.Optimization;

namespace webNews
{
    public class BundleConfig
    {

        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/theme").Include(
                    //Bootstrap Core CSS
                    "~/Theme/bootstrap/dist/css/bootstrap.min.css",
                    //Menu CSS
                    "~/Theme/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.css",
                    //toast CSS
                    "~/Theme/plugins/bower_components/toast-master/css/jquery.toast.css",
                    //morris CSS
                    "~/Theme/plugins/bower_components/morrisjs/morris.css",
                    //chartist CSS
                    "~/Theme/plugins/bower_components/chartist-js/dist/chartist.min.css",
                    "~/Theme/plugins/bower_components/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.css",
                    //Calendar CSS
                    "~/Theme/plugins/bower_components/calendar/dist/fullcalendar.css",
                    //color CSS
                    "~/Theme/css/custom.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                    //All Jquery
                    "~/Theme/plugins/bower_components/jquery/dist/jquery.min.js",
                    //Bootstrap Core JavaScript
                    "~/Theme/bootstrap/dist/js/bootstrap.min.js",
                    //Menu Plugin JavaScript
                    "~/Theme/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.js",
                    //slimscroll JavaScript
                    "~/Theme/js/jquery.slimscroll.js",
                    //Wave Effects
                    "~/Theme/js/waves.js",
                    //chartist chart
                    "~/Theme/plugins/bower_components/chartist-js/dist/chartist.min.js",
                    "~/Theme/plugins/bower_components/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.min.js",
                    //Sparkline chart JavaScript
                    "~/Theme/plugins/bower_components/jquery-sparkline/jquery.sparkline.min.js",
                    //Style Switcher
                    "~/Theme/plugins/bower_components/styleswitcher/jQuery.style.switcher.js",
                    "~/Scripts/Common/language-vi.js",
                    //Custom Theme JavaScript
                    "~/Theme/js/custom.min.js"));          

                     bundles.Add(new ScriptBundle("~/bundles/RoleManage").Include(
                    "~/Scripts/inputmask/jquery.inputmask.bundle.js",
                    "~/Scripts/jquery-validate/jquery.validate.js",
                    "~/Scripts/bootstrap-table/bootstrap-table.js",
                    "~/Scripts/bootbox/bootbox.min.js",
                    "~/Scripts/bootstrap-datetimepicker/moment-with-locales.min.js",
                    "~/Scripts/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js",
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/Common/Service.js",
                    "~/Scripts/Admin/RoleManage/Ctrl.js"
                    ));


                bundles.Add(new ScriptBundle("~/bundles/RoleManagement").Include(
               "~/Scripts/inputmask/jquery.inputmask.bundle.js",
               "~/Scripts/jquery-validate/jquery.validate.js",
               "~/Scripts/bootstrap-table/bootstrap-table.js",
               "~/Scripts/bootbox/bootbox.min.js",
               "~/Scripts/bootstrap-datetimepicker/moment-with-locales.min.js",
               "~/Scripts/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js",
               "~/Scripts/jquery.validate.min.js",
               "~/Scripts/Common/Service.js",
               "~/Scripts/Admin/RoleManagement/RoleManagement.js"
               ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
