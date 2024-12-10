using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Initium.Helpers;
using Initium.Response;
using Initium.Results;

namespace Initium.Filters;

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
        if (context.Result is not ObjectResult { Value: ServiceResult serviceResult }) 
            return;

        // Extract API response attributes from the current action descriptor (method or controller level).
        var actionDescriptor = context.ActionDescriptor;
        var apiResponseAttributes = ApiResponseHelper.GetApiResponseAttributes(actionDescriptor);

        var statusCode = (int?)serviceResult.StatusCode ?? context.HttpContext.Response.StatusCode;
        
        // Find the matching ApiResponseAttribute based on the HTTP status code.
        var matchingAttribute = apiResponseAttributes.FirstOrDefault(attribute =>
            attribute.StatusCode == statusCode);

        // Create an ApiResponse object, including HTTP context details.
        context.Result = ApiResponseBuilder
            .CreateFromContext(context.HttpContext)
            .WithStatusCode(statusCode)
            .WithMessage(serviceResult.Message 
                         ?? matchingAttribute?.Message 
                         ?? ApiResponseHelper.GetDefaultMessageForStatusCode(statusCode))
            .BuildAsJsonResult();
    }
}
