using System.Net;
using Initium.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Initium.Infrastructure.Helpers;

/// <summary>
/// Provides helper methods for logging HTTP request details.
/// </summary>
internal static class LoggingHelper
{
	/// <summary>
	/// Logs an HTTP request with its method, path, status code, and elapsed time.
	/// </summary>
	/// <param name="logger">The logger instance.</param>
	/// <param name="httpContext">The HTTP context of the request.</param>
	/// <param name="statusCode">The response status code.</param>
	/// <param name="elapsedMilliseconds">The elapsed time in milliseconds.</param>
	public static void LogRequest(ILogger logger, HttpContext httpContext, HttpStatusCode statusCode, long elapsedMilliseconds) =>
		logger.LogTrace("{Emoji} HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms.",
			statusCode.IsSuccess() ? "✅" : "🛑",
			httpContext.Request.Method,
			httpContext.Request.Path,
			(int) statusCode,
			elapsedMilliseconds);
}