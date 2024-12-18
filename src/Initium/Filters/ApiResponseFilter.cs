using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Initium.Helpers;
using Initium.Response;
using Initium.Results;
using Microsoft.Extensions.Logging;

namespace Initium.Filters;

/// <summary>
/// Filter responsible for transforming <see cref="ServiceResult"/> objects into a standardized <see cref="ApiResponse"/>.
/// </summary>
internal class ApiResponseFilter(ILogger<ApiResponseFilter> logger) : ActionFilterAttribute
{
    /// <summary>
    /// Called after the action executed, and transforms <see cref="ServiceResult"/> into a standardized <see cref="ApiResponse"/>.
    /// </summary>
    /// <param name="context">The context for the executed action.</param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // Check if the result of the action is an ObjectResult containing a ServiceResult.
        if (context.Result is not ObjectResult { Value: ServiceResult serviceResult }) return;
 
        // Determine the HTTP status code:
		// 1. Use ServiceResult status code if set.
		// 2. Otherwise, use the default based on the HTTP method.
        var statusCode = 
	        serviceResult.StatusCode 
	        ?? ApiResponseHelper.GetDefaultStatusCode(context.HttpContext);

		// Determine the response message:
		// 1. Use ServiceResult message if set.
		// 2. Otherwise, get the message from [ApiResponse] attributes.
		// 3. If none found, use the default message for the StatusCode.
        var message = 
	        serviceResult.Message 
	        ?? GetApiResponseMessage(context, statusCode)
	        ?? ApiResponseHelper.GetDefaultMessageForStatusCode(statusCode);
        
        // Create an ApiResponse object, including HTTP context details.
        context.Result = ApiResponseBuilder
            .CreateFromContext(context.HttpContext)
            .WithStatusCode(statusCode)
            .WithMessage(message)
            // .WithData(serviceResult.Data)
            .WithCustomHeaders(serviceResult.Metadata)
            .BuildAsJsonResult();
    }

    private static string? GetApiResponseMessage(ActionExecutedContext context, HttpStatusCode statusCode) => 
        ApiResponseHelper.GetApiResponseAttributes(context.ActionDescriptor).FirstOrDefault(attribute => attribute.StatusCode == statusCode)?.Message;
}
