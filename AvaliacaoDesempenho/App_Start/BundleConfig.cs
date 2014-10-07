using System.Web.Optimization;

namespace AvaliacaoDesempenho
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Styles/css")
                            .Include("~/Content/Styles/site.css"));

            bundles.Add(new StyleBundle("~/Styles/bootstrap")
                            .Include("~/Content/Styles/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                            .Include("~/Content/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
                            .Include("~/Content/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                            .Include("~/Content/Scripts/jquery.unobtrusive*",
                                     "~/Content/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                            .Include("~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                            .Include("~/Content/Scripts/bootstrap.js",
                                     "~/Content/Scripts/respond.js"));
            
            // TODO
            BundleTable.EnableOptimizations = false;
        }
    }
}