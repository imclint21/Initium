using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Initium.Response;

/// <summary>
/// Provides a fluent builder for creating <see cref="Response.ApiResponse"/> objects.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
internal class ApiResponseBuilder
{
    public ApiResponse ApiResponse { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponseBuilder"/> class.
    /// </summary>
    private ApiResponseBuilder() => ApiResponse = new ApiResponse();

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
        ApiResponse =
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
        ApiResponse.Message = message;
        return this;
    }

    /// <summary>
    /// Sets the HTTP status code for the API response.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to include in the response.</param>
    /// <returns>The current instance of the <see cref="ApiResponseBuilder"/>.</returns>
    public ApiResponseBuilder WithStatusCode(HttpStatusCode statusCode)
    {
        ApiResponse.StatusCode = (int)statusCode;
        return this;
    }

    // public ApiResponseBuilder WithData(object data)
    // {
    //     ApiResponse.Data = data;
    //     return this;
    // }

    /// <summary>
    /// Adds error messages to the API response.
    /// </summary>
    /// <param name="errors">A list of error messages to include in the response.</param>
    /// <returns>The current instance of the <see cref="ApiResponseBuilder"/>.</returns>
    public ApiResponseBuilder WithErrors(params ApiError[] errors)
    {
        ApiResponse.Errors = errors.ToList();
        return this;
    }

    /// <summary>
    /// Adds a custom header to the API response.
    /// </summary>
    /// <param name="headerName">The name of the header to be added.</param>
    /// <param name="headerValue">The value for the specified header.</param>
    /// <returns>The current instance of <see cref="ApiResponseBuilder"/> with the custom header added.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="headerName"/> is null, empty, or consists only of white-space characters.</exception>
    public ApiResponseBuilder WithCustomHeader(string headerName, string headerValue)
    {
        if (string.IsNullOrWhiteSpace(headerName))
            throw new ArgumentException("Header name cannot be null or empty.", nameof(headerName));

        ApiResponse.CustomHeaders ??= new Dictionary<string, string>();
        ApiResponse.CustomHeaders[headerName] = headerValue;
        return this;
    }

    /// <summary>
    /// Adds custom headers to the API response.
    /// </summary>
    /// <param name="headers">A dictionary containing header names as keys and their respective values.</param>
    /// <returns>The current instance of <see cref="ApiResponseBuilder"/> with the added custom headers.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="headers"/> dictionary is null or empty.</exception>
    public ApiResponseBuilder WithCustomHeaders(Dictionary<string, string> headers)
    {
        ApiResponse.CustomHeaders ??= new Dictionary<string, string>();
        foreach (var header in headers) 
            ApiResponse.CustomHeaders[header.Key] = header.Value;

        return this;
    }
    
    /// <summary>
    /// Builds and returns the constructed <see cref="Response.ApiResponse"/>.
    /// </summary>
    /// <returns>The constructed <see cref="Response.ApiResponse"/>.</returns>
    public ApiResponse Build() => ApiResponse;

    /// <summary>
    /// Builds and returns the constructed <see cref="Response.ApiResponse"/> as a <see cref="JsonResult"/>.
    /// </summary>
    /// <returns>A <see cref="JsonResult"/> containing the constructed <see cref="Response.ApiResponse"/>.</returns>
    public JsonResult BuildAsJsonResult() => new(ApiResponse)
    {
        StatusCode = ApiResponse.StatusCode
    };
}
