using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Shaangor.Web
{
	[AttributeUsageAttribute(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple=false)]
	public sealed class LogRequestsAttribute:ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if(filterContext==null) throw new ArgumentNullException("filterContext");
			base.OnActionExecuting(filterContext);

			var request=filterContext.HttpContext.Request;
			var headersBuilder=new StringBuilder();
			for(int i=0;i<request.Headers.Count;i++)
			{
				headersBuilder.AppendFormat("{0}={1};",request.Headers.Keys[i],request.Headers[i].ToString());
			}

			var routeData=request.RequestContext.RouteData;
			var item=new AccessLogItem()
			{
				RequestTimestamp=DateTime.Now,
				RequestType=request.RequestType,
				Url=request.RawUrl,
				IPAddress=request.UserHostAddress,
				Controller=(string)routeData.Values["controller"],
				Action=(string)routeData.Values["action"],
				RequestHeader=headersBuilder.ToString()
			};

			using(var reader=new StreamReader(request.InputStream))
			{
				try
				{
					request.InputStream.Position=0;
					item.RequestBody=reader.ReadToEnd();
				}
				catch(IOException)
				{
					item.RequestBody=string.Empty;
				}
				catch(OutOfMemoryException)
				{
					item.RequestBody=string.Empty;
				}
				finally
				{
					request.InputStream.Position=0;
				}
			}

			filterContext.HttpContext.Response.Filter=new CopySinkWrapperFilter(filterContext.HttpContext.Response.Filter,item);
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if(filterContext==null) throw new ArgumentNullException("filterContext");
			base.OnResultExecuted(filterContext);

			var response=filterContext.HttpContext.Response;
			var sb=new StringBuilder();
			for(int i=0;i<response.Headers.Count;i++)
			{
				sb.AppendFormat("{0}={1};",response.Headers.Keys[i],response.Headers[i].ToString());
			}

			var filter=(CopySinkWrapperFilter)response.Filter;
			var item=filter.AccessLogItem;
			item.ResponseTimestamp=DateTime.Now;
			item.ResponseHeader=sb.ToString();
		}
	}
}
