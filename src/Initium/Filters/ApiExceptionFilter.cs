using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Initium.Exceptions;
using Initium.Response;

namespace Initium.Filters;

/// <summary>
/// A filter that handles exceptions and transforms them into a standardized <see cref="ApiResponse"/>.
/// </summary>
internal class ApiExceptionFilter : IExceptionFilter
{
	/// <summary>
	/// Handles exceptions that occur during the execution of an action.
	/// Transforms known <see cref="ApiException"/> instances into structured API responses
	/// and handles unknown exceptions with a generic error response.
	/// </summary>
	/// <param name="context">The context for the exception.</param>
	public void OnException(ExceptionContext context)
	{
		// Start building the ApiResponse using the Fluent Builder
		var apiResponseBuilder = ApiResponseBuilder.CreateFromContext(context.HttpContext);

		// Use pattern switch to handle exceptions
		context.Result = context.Exception switch
		{
			ApiException ex => apiResponseBuilder
				.WithMessage(ex.Message)
				.WithStatusCode((int)ex.StatusCode)
				.BuildAsJsonResult(),

			NotImplementedException ex => apiResponseBuilder
				.WithMessage(ex.Message)
				.WithStatusCode((int)HttpStatusCode.NotImplemented)
				.BuildAsJsonResult(),

			_ => apiResponseBuilder
				.WithMessage("An unexpected error occurred.")
				.WithStatusCode((int)HttpStatusCode.InternalServerError)
				.BuildAsJsonResult()
		};

		context.ExceptionHandled = true;
	}
}
//
// public static implicit operator ActionResult(ServiceResult result)
// {
// 	if (result.Success && result.StatusCode == HttpStatusCode.NoContent)
// 	{
// 		return new NoContentResult();
// 	}
//
// 	return new ObjectResult(result)
// 	{
// 		StatusCode = (int)result.StatusCode
// 	};
// }
