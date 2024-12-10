using System.Net;

namespace Initium.Infrastructure;

/// <summary>
/// Represents a base result model, providing common properties to indicate the outcome of an operation.
/// </summary>
/// <remarks>
/// This class can be extended or used as a base for more specific result types,
/// providing details such as success state, messages, status codes, and metadata.
/// </remarks>
[Obsolete("Move to internal?")]
public abstract class BaseResult
{
	/// <summary>
	/// Gets or sets a value indicating whether the operation was successful.
	/// </summary>
	public bool Success { get; set; }

	/// <summary>
	/// Gets or sets the message providing additional details about the operation or its outcome.
	/// </summary>
	public string? Message { get; set; }

	/// <summary>
	/// Gets or sets the HTTP status code associated with the result, indicating the outcome of the operation.
	/// </summary>
	public HttpStatusCode? StatusCode { get; set; }
	
	/// <summary>
	/// Gets or sets metadata associated with the result, which can be mapped to HTTP headers.
	/// </summary>
	public Dictionary<string, string>? Metadata { get; set; }
}
