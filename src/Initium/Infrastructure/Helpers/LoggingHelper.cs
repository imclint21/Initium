using System.Net;
using Initium.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Initium.Infrastructure.Helpers;

internal static class LoggingHelper
{
	// HTTP GET /health responded 200 in 8.9988 ms
	public static void LogRequest(ILogger logger, HttpContext httpContext, HttpStatusCode statusCode, long elapsedMilliseconds) =>
		logger.LogTrace("{Emoji} HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms.", 
			statusCode.IsSuccess() ? "✅" : "🛑", 
			httpContext.Request.Method, 
			httpContext.Request.Path, 
			(int) statusCode,
			// statusCode,
			elapsedMilliseconds);
}