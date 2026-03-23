using Tapper;
using YamlDotNet.Serialization;

namespace Initium.Response;

/// <summary>
/// Contains details about the HTTP request that generated an API response.
/// </summary>
[TranspilationSource]
public class RequestDetails
{
	/// <summary>
	/// Gets or sets the client's IP address.
	/// </summary>
	public string? ClientIp { get; set; }

	/// <summary>
	/// Gets or sets the request endpoint path.
	/// </summary>
	public string? Endpoint { get; set; }

	/// <summary>
	/// Gets or sets the timestamp of the request.
	/// </summary>
	[YamlIgnore]
	public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

	/// <summary>
	/// Gets or sets the client's User-Agent string.
	/// </summary>
	public string? UserAgent { get; set; }

	/// <summary>
	/// Gets the request timestamp as a Unix epoch in milliseconds.
	/// </summary>
	public long Timestamp => Date.ToUnixTimeMilliseconds();

	/// <summary>
	/// Gets or sets the correlation identifier for request tracing.
	/// </summary>
	public string? CorrelationId { get; set; }
}