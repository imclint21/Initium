namespace Initium.Request;

public abstract class BaseRequestWithValidator<TValidator> where TValidator : new()
{
	protected readonly TValidator Validator = new();
}
