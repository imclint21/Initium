using System.Net;
using FluentValidation;
using Initium.Request;
using Initium.Response;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Infrastructure.Filters;

/// <summary>
/// A filter that performs implicit validation on actions using requests derived from <see cref="BaseRequestWithValidator{T}"/>.
/// </summary>
internal class ImplicitValidationFilter : IActionFilter
{
    /// <summary>
    /// Called before the action executes, to validate the action arguments.
    /// </summary>
    /// <param name="context">The context for the action execution.</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var argument = context.ActionArguments.Values.FirstOrDefault();
        if (argument == null) return;

        var argumentType = argument.GetType();
        var baseType = argumentType.BaseType;

        if (baseType is not { IsGenericType: true } || baseType.GetGenericTypeDefinition() != typeof(BaseRequestWithValidator<>)) return;

        var validatorType = baseType.GetGenericArguments().FirstOrDefault();
        if (validatorType == null || Activator.CreateInstance(validatorType) is not IValidator validator) return;

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
    /// Called after the action executes. No operation is performed in this implementation.
    /// </summary>
    /// <param name="context">The context for the executed action.</param>
    public void OnActionExecuted(ActionExecutedContext context) { }
}
