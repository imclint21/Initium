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
		// Create a base ApiResponse object, including HTTP context details.
		var apiResponse = ApiResponse.CreateFromHttpContext(context.HttpContext);
		switch (context.Exception)
		{
			default:
				apiResponse.Message = "An unexpected error occurred.";
				apiResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
				break;
			
			case ApiException ex:
				// TODO: Not persistent: "Exception of type 'Initium.Exceptions.ApiException' was thrown."
				apiResponse.Message = ex.Message;
				apiResponse.StatusCode = (int)ex.StatusCode;
				break;
			
			case NotImplementedException ex:
				apiResponse.Message = ex.Message;
				apiResponse.StatusCode = (int)HttpStatusCode.NotImplemented;
				break;
		}

		context.ExceptionHandled = true;
		context.Result = new JsonResult(apiResponse)
		{
			StatusCode = apiResponse.StatusCode
		};
	}
}