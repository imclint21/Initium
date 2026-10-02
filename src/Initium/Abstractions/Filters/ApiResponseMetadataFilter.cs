using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Abstractions.Filters;

/// <summary>
/// Applies the status code and headers that an unwrapped <see cref="Results.ServiceResult"/> stashed in
/// <see cref="ApiResponseContext"/>. This is what lets <c>Service.Create(...).UnwrapOrThrow()</c> return the bare
/// entity as the body while the result's 201 status and <c>Location</c> header still reach the response.
/// </summary>
internal class ApiResponseMetadataFilter(IHttpContextAccessor httpContextAccessor) : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context) => ApiResponseContext.Use(httpContextAccessor);

	public void OnActionExecuted(ActionExecutedContext context)
	{
		var httpContext = context.HttpContext;

		// Headers (e.g. Location) are safe to write straight onto the response now.
		var metadata = ApiResponseContext.ReadMetadata(httpContext);
		if (metadata is not null)
			foreach (var header in metadata)
				httpContext.Response.Headers[header.Key] = header.Value;

		// The status code must ride on the result itself — setting Response.StatusCode here would be
		// overwritten when the ObjectResult executes. Stamp the stashed code onto the pending result.
		if (ApiResponseContext.ReadStatusCode(httpContext) is { } statusCode && context.Result is ObjectResult objectResult)
			objectResult.StatusCode = (int)statusCode;
	}
}
