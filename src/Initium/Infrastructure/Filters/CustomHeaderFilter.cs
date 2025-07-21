using Initium.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Infrastructure.Filters;

internal class CustomHeaderFilter : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context)
	{
	}
	
	public void OnActionExecuted(ActionExecutedContext context) =>
		context.ActionDescriptor.EndpointMetadata
			.OfType<CustomHeaderAttribute>()
			.ToList()
			.ForEach(customHeader => context.HttpContext.Response.Headers[customHeader.HeaderName] = customHeader.HeaderValue);
}