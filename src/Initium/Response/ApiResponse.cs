using System.Text.Json.Serialization;
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
	
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public IEnumerable<ApiError>? Errors { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
	public Dictionary<string, string>? CustomHeaders { get; set; }
}