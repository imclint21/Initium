using Initium.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Infrastructure.Filters;

/// <summary>
/// Applies <see cref="CustomHeaderAttribute"/> values to the HTTP response headers.
/// </summary>
internal class CustomHeaderFilter : IActionFilter
{
	/// <inheritdoc />
	public void OnActionExecuting(ActionExecutingContext context)
	{
	}

	/// <inheritdoc />
	public void OnActionExecuted(ActionExecutedContext context) =>
		context.ActionDescriptor.EndpointMetadata
			.OfType<CustomHeaderAttribute>()
			.ToList()
			.ForEach(customHeader => context.HttpContext.Response.Headers[customHeader.HeaderName] = customHeader.HeaderValue);
}