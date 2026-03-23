using System.Diagnostics;
using System.Net;
using Initium.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Initium.Infrastructure.Filters;

/// <summary>
/// Logs HTTP request details including method, path, status code, and elapsed time.
/// </summary>
public class LoggingFilter : IResultFilter
{
	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	/// <inheritdoc />
	public void OnResultExecuting(ResultExecutingContext context) { }

	/// <inheritdoc />
	public void OnResultExecuted(ResultExecutedContext context)
	{
		var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LoggingFilter>>();
		var statusCode = (HttpStatusCode)context.HttpContext.Response.StatusCode;
		LoggingHelper.LogRequest(logger, context.HttpContext, statusCode, _stopwatch.ElapsedMilliseconds);
	}
}