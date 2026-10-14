using FluentValidation;

namespace Initium.Request;

/// <summary>
/// Base class for request DTOs that declare their validator type. The type is a marker the validation filter reads
/// to find the validator: it resolves it from the request's services (so the validator can inject dependencies and
/// run asynchronous rules), falling back to a parameterless instance when none is registered.
/// </summary>
/// <typeparam name="TValidator">The FluentValidation validator for this request.</typeparam>
public abstract class BaseRequestWithValidator<TValidator> where TValidator : IValidator;
