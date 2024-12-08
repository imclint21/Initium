using Tapper;

namespace Initium.Response;

[TranspilationSource]
public class RequestDetails
{
	public string? ClientIp { get; set; }
	public string? Endpoint { get; set; }
	public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
	public string? UserAgent { get; set; }
	public long Timestamp => Date.ToUnixTimeMilliseconds();
	public string? CorrelationId { get; set; }
}