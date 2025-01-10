using System.Net;
using Initium.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Initium.Helpers;

public static class LoggingHelper
{
	public static void LogRequest(ILogger logger, HttpContext httpContext, HttpStatusCode statusCode, long elapsedMilliseconds) => 
		logger.LogTrace("{Emoji} [{Method}] -> {Path} -> [{StatusCode} {StatusCodeText} in {ElapsedMilliseconds}ms]", 
			statusCode.IsSuccess() ? "✅" : "🛑", 
			httpContext.Request.Method, 
			httpContext.Request.Path, 
			(int) statusCode,
			statusCode,
			elapsedMilliseconds);
}