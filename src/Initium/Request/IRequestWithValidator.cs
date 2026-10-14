using FluentValidation;

namespace Initium.Request;

/// <summary>
/// Marks a request DTO with the validator the validation filter should run for it. The filter resolves that
/// validator from the request's services (so it can inject dependencies and run asynchronous rules), falling back
/// to a parameterless instance when none is registered. An interface so a request can still derive from its own
/// base class and only declare which validator applies.
/// </summary>
/// <typeparam name="TValidator">The FluentValidation validator for this request.</typeparam>
public interface IRequestWithValidator<TValidator> where TValidator : IValidator;
