using System.Text.Json.Serialization;
using Tapper;

namespace Initium.Constants;

/// <summary>
/// Represents different levels of severity for categorizing events, states, or conditions.
/// These levels can be used in various contexts to indicate the importance or criticality of an event.
/// </summary>
[TranspilationSource]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SeverityLevels
{
	/// <summary>
	/// Represents the most detailed level of information. Typically includes extensive and potentially sensitive details.
	/// This level should be used with caution in sensitive environments.
	/// </summary>
	Trace,

	/// <summary>
	/// Represents general informational messages. Often used for routine insights or operational context.
	/// </summary>
	Information,

	/// <summary>
	/// Represents situations that indicate potential issues or irregularities but do not disrupt operations.
	/// </summary>
	Warning,

	/// <summary>
	/// Represents conditions that indicate an error or failure in a specific process or operation.
	/// Typically requires attention to resolve.
	/// </summary>
	Error
}