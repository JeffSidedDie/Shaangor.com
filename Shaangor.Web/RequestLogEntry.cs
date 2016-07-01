using System;

namespace Shaangor.Web
{
	public class RequestLogEntry
	{
		public string UserName { get; set; }
		public string IpAddress { get; set; }
		public Uri Uri { get; set; }
		public string Method { get; set; }

		public string RequestContentType { get; set; }
		public string RequestHeaders { get; set; }
		public string RequestBody { get; set; }
		public DateTime RequestTimestamp { get; set; }

		public string Controller { get; set; }
		public string Action { get; set; }

		public int ResponseStatusCode { get; set; }
		public string ResponseContentType { get; set; }
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
