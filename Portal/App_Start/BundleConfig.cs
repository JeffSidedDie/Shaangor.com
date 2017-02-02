using System.Web;
using System.Web.Optimization;

namespace Portal
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/js").Include(
				"~/Scripts/jquery-{version}.js",
				"~/Scripts/bootstrap.js"
			));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css",
				"~/Content/print.css"
			));

			bundles.Add(new StyleBundle("~/Content/css/resume").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css",
				"~/Content/print.css",
				"~/Content/resume.css",
				"~/Content/font-awesome.css"
			));

			BundleTable.EnableOptimizations=true;
		}
	}
}
