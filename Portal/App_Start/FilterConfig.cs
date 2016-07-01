using Shaangor.Web;
using System.Web;
using System.Web.Mvc;
using System;

namespace Portal
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new LogRequestsActionFilter(() => new MockRequestLogEntryRepository()));
			//filters.Add(new LogRequestsResultFilter(()=>new MockRequestLogEntryRepository()));
		}

		private class MockRequestLogEntryRepository:IRequestLogEntryRepository
		{
			public void Add(RequestLogEntry requestLogEntry)
			{
			}

			public void Commit()
			{
			}
		}
	}
}
