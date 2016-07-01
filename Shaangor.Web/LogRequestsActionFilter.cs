using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Shaangor.Web
{
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple = false)]
	public sealed class LogRequestsAttribute:Attribute
	{
	}

	public class LogRequestsActionFilter:IActionFilter
	{
		private static Type LogRequestsAttributeType { get; } = typeof(LogRequestsAttribute);
		private static bool ShouldLogRequests { get; } = ConfigurationManager.AppSettings["EnableRequestLogging"]==bool.TrueString;

		private readonly Func<IRequestLogEntryRepository> _repositoryFactory;

		public LogRequestsActionFilter(Func<IRequestLogEntryRepository> repositoryFactory)
		{
			if(repositoryFactory==null) throw new ArgumentNullException(nameof(repositoryFactory));

			_repositoryFactory=repositoryFactory;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if(ShouldLogRequests&&(filterContext.ActionDescriptor.GetCustomAttributes(LogRequestsAttributeType,true).Any()||filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(LogRequestsAttributeType,true).Any()))
			{
				var request=filterContext.HttpContext.Request;
				var routeData=request.RequestContext.RouteData;
				var logEntry=new RequestLogEntry()
				{
					RequestTimestamp=DateTime.Now,
					RequestMethod=request.RequestType,
					Uri=request.Url,
					IPAddress=request.UserHostAddress,
					Controller=(string)routeData.Values["controller"],
					Action=(string)routeData.Values["action"],
					RequestHeaders=request.Headers.Concatenate()
				};

				using(var reader = new StreamReader(request.InputStream))
				{
					try
					{
						request.InputStream.Position=0;
						logEntry.RequestBody=reader.ReadToEnd();
					}
					catch(IOException)
					{
						logEntry.RequestBody=string.Empty;
					}
					catch(OutOfMemoryException)
					{
						logEntry.RequestBody=string.Empty;
					}
					finally
					{
						request.InputStream.Position=0;
					}
				}
				
				var responseFilter=new CopySinkWrapperStream(filterContext.HttpContext.Response.Filter,filterContext.HttpContext.Response.ContentEncoding);
				responseFilter.BeforeClose+=(sender,args) =>
				{
					var filter=sender as CopySinkWrapperStream;

					var response=filterContext.HttpContext.Response;
					logEntry.ResponseTimestamp=DateTime.Now;
					logEntry.ResponseHeaders=response.Headers.Concatenate();
					logEntry.ResponseBody=filter.ReadCopyToEnd();

					var repository=_repositoryFactory();
					repository.Add(logEntry);
					repository.Commit();
				};
				filterContext.HttpContext.Response.Filter=responseFilter;
			}
		}

		public void OnActionExecuted(ActionExecutedContext filterContext) { }
	}

	internal static class HeaderCollectionExtensions
	{
		public static string Concatenate(this NameValueCollection headers)
		{
			var headersBuilder=new StringBuilder();
			for(int i = 0;i<headers.Count;i++)
			{
				headersBuilder.AppendFormat(CultureInfo.InvariantCulture,"{0}={1};",headers.Keys[i],headers[i].ToString());
			}
			return headersBuilder.ToString();
		}
	}
}
