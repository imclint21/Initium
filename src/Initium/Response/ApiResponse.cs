using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Tapper;

namespace Initium.Response;

/// <summary>
/// Represents a standardized API response structure.
/// </summary>
[TranspilationSource]
public class ApiResponse
{
	/// <summary>
	/// Gets or sets an optional message describing the response.
	/// </summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Message { get; set; }

	/// <summary>
	/// Gets or sets the HTTP status code of the response.
	/// </summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public int StatusCode { get; set; }

	/// <summary>
	/// Gets or sets details about the request that generated this response.
	/// </summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public RequestDetails? RequestDetails { get; set; } = new();

	/// <summary>
	/// Creates an <see cref="ApiResponse"/> instance from the provided <see cref="HttpContext"/>.
	/// </summary>
	/// <param name="httpContext">The current HTTP context from which details will be extracted.</param>
	/// <returns>An instance of <see cref="ApiResponse"/> populated with request details.</returns>
	public static ApiResponse CreateFromHttpContext(HttpContext httpContext) => new()
	{
		RequestDetails = new RequestDetails
		{
			ClientIp = httpContext.Connection.RemoteIpAddress?.ToString(),
			Endpoint = httpContext.Request.Path,
			UserAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault(),
			CorrelationId = httpContext.TraceIdentifier
		}
	};
}