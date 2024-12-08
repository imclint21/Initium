using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Tapper;

namespace Initium.Response;

[TranspilationSource]
public class ApiResponse
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Message { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public int StatusCode { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public RequestDetails? RequestDetails { get; set; } = new();
	
	public static ApiResponse CreateFromHttpContext(HttpContext httpContext) => new()
	{
		StatusCode = httpContext.Response.StatusCode, 
		RequestDetails = new RequestDetails
		{
			ClientIp = httpContext.Connection.RemoteIpAddress?.ToString(),
			Endpoint = httpContext.Request.Path,
			UserAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault(),
			CorrelationId = httpContext.TraceIdentifier
		}
	};
}