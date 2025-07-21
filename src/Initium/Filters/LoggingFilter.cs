using System.Diagnostics;
using System.Net;
using Initium.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Initium.Filters;

public class LoggingFilter : IResultFilter
{
	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	public void OnResultExecuting(ResultExecutingContext context) { }

	public void OnResultExecuted(ResultExecutedContext context)
	{
		var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LoggingFilter>>();
		var statusCode = (HttpStatusCode)context.HttpContext.Response.StatusCode;
		LoggingHelper.LogRequest(logger, context.HttpContext, statusCode, _stopwatch.ElapsedMilliseconds);
	}
}