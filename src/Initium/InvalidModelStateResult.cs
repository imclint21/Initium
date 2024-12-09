using Initium.Response;
using Microsoft.AspNetCore.Mvc;

namespace Initium;

/// <summary>
/// Represents a custom result for invalid model state, returned as a JSON response with standardized API response structure.
/// </summary>
public class InvalidModelStateResult : JsonResult
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidModelStateResult"/> class with the provided action context.
	/// </summary>
	/// <param name="actionContext">The context of the action where the model validation failed.</param>
	public InvalidModelStateResult(ActionContext actionContext) : base(actionContext)
	{
		// Create a standardized API response using the HTTP context.
		var apiResponse = ApiResponse.CreateFromHttpContext(actionContext.HttpContext);

		// Set the response properties.
		apiResponse.Message = "One or more validation errors occurred.";
		apiResponse.StatusCode = 400;

		// Assign the response details to the JsonResult properties.
		StatusCode = apiResponse.StatusCode;
		Value = apiResponse;
	}
}