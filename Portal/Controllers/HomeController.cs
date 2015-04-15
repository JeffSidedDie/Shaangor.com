using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shaangor.Web;

namespace Portal.Controllers
{
	[LogRequests]
	public class HomeController:Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}