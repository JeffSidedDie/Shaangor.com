using System;

namespace Shaangor.Web
{
	public class RequestLogEntry
	{
		public string IPAddress { get; set; }
		public Uri Uri { get; set; }
		public string RequestMethod { get; set; }
		public string RequestHeaders { get; set; }
		public string RequestBody { get; set; }
		public DateTime RequestTimestamp { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public string ResponseHeaders { get; set; }
		public string ResponseBody { get; set; }
		public DateTime ResponseTimestamp { get; set; }
	}

	public interface IRequestLogEntryRepository
	{
		void Add(RequestLogEntry requestLogEntry);
		void Commit();
	}
}
