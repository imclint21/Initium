using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Initium.Response;

/// <summary>
/// Provides a fluent builder for creating <see cref="ApiResponse"/> objects.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal class ApiResponseBuilder
{
    private readonly ApiResponse _apiResponse;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponseBuilder"/> class.
    /// </summary>
    private ApiResponseBuilder() => _apiResponse = new ApiResponse();

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponseBuilder"/>.
    /// </summary>
    /// <returns>A new instance of the <see cref="ApiResponseBuilder"/>.</returns>
    public static ApiResponseBuilder Create() => new();

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponseBuilder"/> and initializes it with HTTP context details.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> from which to extract details for the API response.</param>
    /// <returns>A new instance of the <see cref="ApiResponseBuilder"/> initialized with context details.</returns>
    public static ApiResponseBuilder CreateFromContext(HttpContext context) => new()
    {
        _apiResponse =
        {
            RequestDetails = new RequestDetails
            {
                ClientIp = context.Connection.RemoteIpAddress?.ToString(),
                Endpoint = context.Request.Path,
                UserAgent = context.Request.Headers.UserAgent.FirstOrDefault(),
                CorrelationId = context.TraceIdentifier
            }
        }
    };

    /// <summary>
    /// Sets the message for the API response.
    /// </summary>
    /// <param name="message">The message to include in the response.</param>
    /// <returns>The current instance of the <see cref="ApiResponseBuilder"/>.</returns>
    public ApiResponseBuilder WithMessage(string message)
    {
        _apiResponse.Message = message;
        return this;
    }

    /// <summary>
    /// Sets the HTTP status code for the API response.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to include in the response.</param>
    /// <returns>The current instance of the <see cref="ApiResponseBuilder"/>.</returns>
    public ApiResponseBuilder WithStatusCode(HttpStatusCode statusCode)
    {
        _apiResponse.StatusCode = (int)statusCode;
        return this;
    }

    /// <summary>
    /// Adds error messages to the API response.
    /// </summary>
    /// <param name="errors">A list of error messages to include in the response.</param>
    /// <returns>The current instance of the <see cref="ApiResponseBuilder"/>.</returns>
    public ApiResponseBuilder WithErrors(params ApiError[] errors)
    {
        _apiResponse.Errors = errors.ToList();
        return this;
    }

    /// <summary>
    /// Adds a list of validation errors to the API response.
    /// </summary>
    /// <param name="validationResultErrors">The list of validation failures to include in the response.</param>
    /// <returns>The current instance of the <see cref="ApiResponseBuilder"/>.</returns>
    public ApiResponseBuilder WithErrors(List<ValidationFailure> validationResultErrors)
    {
        _apiResponse.Errors = validationResultErrors
            .Select(validationFailure => new ApiError(validationFailure.ErrorCode, validationFailure.ErrorMessage))
            .ToList();
        return this;
    }

    /// <summary>
    /// Builds and returns the constructed <see cref="ApiResponse"/>.
    /// </summary>
    /// <returns>The constructed <see cref="ApiResponse"/>.</returns>
    public ApiResponse Build() => _apiResponse;

    /// <summary>
    /// Builds and returns the constructed <see cref="ApiResponse"/> as a <see cref="JsonResult"/>.
    /// </summary>
    /// <returns>A <see cref="JsonResult"/> containing the constructed <see cref="ApiResponse"/>.</returns>
    public JsonResult BuildAsJsonResult() => new(_apiResponse)
    {
        StatusCode = _apiResponse.StatusCode
    };

    public ApiResponseBuilder WithCustomHeader(string headerName, string headerValue)
    {
        if (string.IsNullOrWhiteSpace(headerName))
            throw new ArgumentException("Header name cannot be null or empty.", nameof(headerName));

        _apiResponse.CustomHeaders ??= new Dictionary<string, string>();
        _apiResponse.CustomHeaders[headerName] = headerValue;
        return this;
    }
}
