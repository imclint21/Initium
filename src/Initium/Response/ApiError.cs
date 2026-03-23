using Tapper;

namespace Initium.Response;

/// <summary>
/// Represents a structured error with a code and description, used in API error responses.
/// </summary>
[TranspilationSource]
public class ApiError
{
	/// <summary>
	/// Gets or sets the error code.
	/// </summary>
	public string? Code { get; set; }

	/// <summary>
	/// Gets or sets the error description.
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// Initializes a new empty instance of <see cref="ApiError"/>.
	/// </summary>
	public ApiError()
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="ApiError"/> with the specified code and description.
	/// </summary>
	/// <param name="code">The error code.</param>
	/// <param name="description">The error description.</param>
	public ApiError(string code, string description)
	{
		Code = code;
		Description = description;
	}
}