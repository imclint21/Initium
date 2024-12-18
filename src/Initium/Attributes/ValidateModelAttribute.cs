using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentValidation;
using Initium.Response;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Attributes;

/// <summary>
/// An attribute that performs model validation using a specified FluentValidation validator.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ValidateModelAttribute(Type validatorType) : Attribute, IActionFilter
{
	/// <summary>
	/// Called before the action method executes. Validates the model using the specified validator.
	/// </summary>
	/// <param name="context">The context for the action execution.</param>
	public void OnActionExecuting(ActionExecutingContext context)
	{
		var argument = context.ActionArguments.Values.FirstOrDefault();
		if (argument == null) return;

		if (Activator.CreateInstance(validatorType) is not IValidator validator) return;

		var validationContext = new ValidationContext<object>(argument);
		var validationResult = validator.Validate(validationContext);

		if (validationResult.IsValid) return;
		
		context.Result = ApiResponseBuilder
			.CreateFromContext(context.HttpContext)
			.WithMessage("One or more validation errors occurred.")
			.WithStatusCode(HttpStatusCode.BadRequest)
			.WithErrors(validationResult.Errors
				.Select(validationFailure => new ApiError(validationFailure.ErrorCode, validationFailure.ErrorMessage))
				.ToArray())
			.BuildAsJsonResult();
	}

	/// <summary>
	/// Called after the action method executes. No operation is performed in this implementation.
	/// </summary>
	/// <param name="context">The context for the executed action.</param>
	public void OnActionExecuted(ActionExecutedContext context) { }
}