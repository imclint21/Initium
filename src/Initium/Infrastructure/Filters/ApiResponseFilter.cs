using System.Net;
using Initium.Infrastructure.Helpers;
using Initium.Response;
using Initium.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Infrastructure.Filters;

/// <summary>
/// Filter responsible for transforming <see cref="ServiceResult"/> objects into a standardized <see cref="ApiResponse"/>.
/// </summary>
internal class ApiResponseFilter : ActionFilterAttribute
{
    /// <summary>
    /// Called after the action executed, and transforms <see cref="ServiceResult"/> into a standardized <see cref="ApiResponse"/>.
    /// </summary>
    /// <param name="context">The context for the executed action.</param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // Check if the result of the action is an ObjectResult containing a ServiceResult.
        if (context.Result is not ObjectResult { Value: ServiceResult serviceResult }) return;
        if (string.IsNullOrWhiteSpace(serviceResult.Message)) serviceResult.Message = null;

        // Start building the ApiResponse using the Fluent Builder
        var apiResponseBuilder = ApiResponseBuilder.CreateFromContext(context.HttpContext);

        // Determine the HTTP status code:
        // 1. Use ServiceResult status code if set.
        // 2. Otherwise, use the default based on the HTTP method.
        var statusCode =
	        serviceResult.StatusCode
	        ?? ApiResponseHelper.GetDefaultStatusCode(context.HttpContext);

        // Determine the response message:
        // 1. Use ServiceResult message if set.
        // 2. Otherwise, get the message from [ApiResponse] attributes.
        // 3. If none is found, use the default message for the StatusCode.
        var message =
	        serviceResult.Message 
	        ?? ApiResponseHelper.GetApiResponseMessage(context.ActionDescriptor, statusCode) 
	        ?? ApiResponseHelper.GetDefaultMessageForStatusCode(statusCode);

        // Determine the appropriate response based on the status code:
        // - For 204 (No Content) and 304 (Not Modified), set a StatusCodeResult without a response body.
        // - For other status codes, construct a standardized ApiResponse object including HTTP context details.
        context.Result = statusCode switch
        {
	        HttpStatusCode.NoContent or HttpStatusCode.NotModified => new StatusCodeResult((int)statusCode),
	        _ => apiResponseBuilder
		        .WithStatusCode(statusCode)
		        .WithMessage(message)
		        .BuildAsJsonResult()
        };
    }
}
