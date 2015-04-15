using System;

namespace Shaangor.Web
{
	public class AccessLogItem
	{
		public long Id { get; set; }
		public string IPAddress { get; set; }
		public string Url { get; set; }
		public string RequestType { get; set; }
		public string RequestHeader { get; set; }
		public string RequestBody { get; set; }
		public DateTime RequestTimestamp { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public string ResponseHeader { get; set; }
		public string ResponseBody { get; set; }
		public DateTime ResponseTimestamp { get; set; }
	}
}
