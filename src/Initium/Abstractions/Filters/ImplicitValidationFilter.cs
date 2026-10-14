using System.Net;
using FluentValidation;
using Initium.Request;
using Initium.Response;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Initium.Abstractions.Filters;

/// <summary>
/// A filter that performs implicit validation on actions whose request declares a validator through
/// <see cref="IRequestWithValidator{T}"/> (or the <see cref="BaseRequestWithValidator{T}"/> base class). The
/// validator is taken from the request's service provider when one is registered there (so it can inject
/// dependencies and run asynchronous rules), and otherwise built with its parameterless constructor.
/// </summary>
internal class ImplicitValidationFilter : IAsyncActionFilter
{
    /// <summary>
    /// Called before the action executes, to validate the action arguments; short-circuits with a 400 when invalid.
    /// </summary>
    /// <param name="context">The context for the action execution.</param>
    /// <param name="next">The delegate to execute the next filter or the action itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var argument = context.ActionArguments.Values.FirstOrDefault();
        if (argument == null)
        {
            await next();
            return;
        }

        var validatorMarker = ValidatorMarker(argument.GetType());
        if (validatorMarker == null)
        {
            await next();
            return;
        }

        var validatorType = validatorMarker.GetGenericArguments().FirstOrDefault();
        var validator = Resolve(context.HttpContext.RequestServices, validatorType);
        if (validator == null)
        {
            await next();
            return;
        }

        var validationContext = new ValidationContext<object>(argument);
        var validationResult = await validator.ValidateAsync(validationContext);

        if (validationResult.IsValid)
        {
            await next();
            return;
        }

        context.Result = ApiResponseBuilder
            .CreateFromContext(context.HttpContext)
            .WithMessage("One or more validation errors occurred.")
            .WithStatusCode(HttpStatusCode.BadRequest)
            .WithErrors(validationResult.Errors
                .Select(validationFailure => new ApiError(validationFailure.ErrorCode, validationFailure.ErrorMessage))
                .ToArray())
            .BuildAsJsonResult();
    }

    // The IRequestWithValidator<> the request declares (preferred), or the BaseRequestWithValidator<> in its
    // inheritance chain, or null when it carries neither. The interface lets a request keep its own base class and
    // still declare a validator; the base class is kept for requests that derive from it directly.
    private static Type? ValidatorMarker(Type requestType)
    {
        var marker = requestType.GetInterfaces()
            .FirstOrDefault(contract => contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IRequestWithValidator<>));
        if (marker != null)
            return marker;

        for (var type = requestType.BaseType; type != null; type = type.BaseType)
            if (type is { IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(BaseRequestWithValidator<>))
                return type;

        return null;
    }

    // The registered validator for this request type (resolved so it can inject dependencies), or a parameterless
    // instance of the declared validator type when none is registered. Null when neither is available.
    private static IValidator? Resolve(IServiceProvider services, Type? validatorType)
    {
        if (validatorType == null) return null;

        var requestType = validatorType.BaseType?.GetGenericArguments().FirstOrDefault();
        if (requestType != null && services.GetService(typeof(IValidator<>).MakeGenericType(requestType)) is IValidator registered)
            return registered;

        return Activator.CreateInstance(validatorType) as IValidator;
    }
}
