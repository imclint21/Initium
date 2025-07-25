using System.Net;
using Initium.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Initium.Infrastructure.Results;

/// <summary>
/// Represents a custom result for invalid model state, returned as a JSON response with standardized API response structure.
/// </summary>
internal class InvalidModelStateResult : JsonResult
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidModelStateResult"/> class with the provided action context.
	/// </summary>
	/// <param name="actionContext">The context of the action where the model validation failed.</param>
	public InvalidModelStateResult(ActionContext actionContext) : base(actionContext)
	{
		var validationErrors = actionContext.ModelState
			.Where(ms => ms.Value != null && ms.Value.Errors.Any())
			.SelectMany(ms => ms.Value?.Errors.Select(error => 
				new ApiError(ms.Key, error.ErrorMessage)) ?? [])
			.ToArray();
		
		// Create a standardized API response using the HTTP context.
		StatusCode = StatusCodes.Status400BadRequest;
		Value = ApiResponseBuilder.CreateFromContext(actionContext.HttpContext)
			.WithMessage("One or more validation errors occurred.")
			.WithStatusCode(HttpStatusCode.BadRequest)
			.WithErrors(validationErrors)
			.Build();
	}
}