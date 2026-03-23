namespace Initium.Request;

/// <summary>
/// Base class for request DTOs that include a built-in validator instance.
/// </summary>
/// <typeparam name="TValidator">The validator type to instantiate.</typeparam>
public abstract class BaseRequestWithValidator<TValidator> where TValidator : new()
{
	/// <summary>
	/// The validator instance for this request.
	/// </summary>
	protected readonly TValidator Validator = new();
}
