using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Initium.Exceptions;
using Initium.Helpers;
using Initium.Response;
using Microsoft.Extensions.Logging;

namespace Initium.Filters;

/// <summary>
/// A filter that handles exceptions and transforms them into a standardized <see cref="ApiResponse"/>.
/// </summary>
internal class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
	/// <summary>
	/// Handles exceptions that occur during the execution of an action.
	/// Transforms known <see cref="ApiException"/> instances into structured API responses
	/// and handles unknown exceptions with a generic error response.
	/// </summary>
	/// <param name="context">The context for the exception.</param>
	public void OnException(ExceptionContext context)
	{
		// Create a stopwatch for logs.
		var stopwatch = Stopwatch.StartNew();
		
		// Start building the ApiResponse using the Fluent Builder
		var apiResponseBuilder = ApiResponseBuilder.CreateFromContext(context.HttpContext);
		
		// Determine the HTTP status code:
		var statusCode = context.Exception switch
		{
			ApiException exception => exception.StatusCode,
			NotImplementedException => HttpStatusCode.NotImplemented,
			_ => HttpStatusCode.InternalServerError 
		};

		// Determine the response message:
		// 1. Use the exception's message if it's not null or whitespace.
		// 2. Otherwise, get the message from [ApiResponse] attributes.
		// 3. If none is found, use the default message for the StatusCode.
		var message = 
			(context.Exception is ApiException apiException ? apiException.CustomMessage : context.Exception.Message)
			?? ApiResponseHelper.GetApiResponseMessage(context.ActionDescriptor, statusCode)
			?? ApiResponseHelper.GetDefaultMessageForStatusCode(statusCode);

		// Construct a standardized ApiResponse object including HTTP context details.
		context.ExceptionHandled = true;
		context.Result = apiResponseBuilder
			.WithStatusCode(statusCode)
			.WithMessage(message)
			.BuildAsJsonResult();
		
		// Logs the details of the current HTTP request.
		LoggingHelper.LogRequest(logger, context.HttpContext, statusCode, stopwatch.ElapsedMilliseconds);
	}
}