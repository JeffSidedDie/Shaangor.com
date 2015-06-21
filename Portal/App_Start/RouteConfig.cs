using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("Resume","Resume",new { controller="Home",action="Resume" });

			routes.MapRoute(
			    name :"Default",
			    url :"{controller}/{action}/{id}",
			    defaults :new { controller="Home",action="Index",id=UrlParameter.Optional }
			);
		}
	}
}
