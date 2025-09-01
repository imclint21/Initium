using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Initium.Exceptions;

/// <summary>
/// Represents an exception specific to API behavior with additional context like HTTP status code and custom messages.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class ApiException : Exception
{
	public string? CustomMessage { get; set; }
	public HttpStatusCode StatusCode { get; }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="ApiException"/> class with a specified HTTP status code, message, and optional inner exception.
	/// </summary>
	/// <param name="statusCode">The HTTP status code associated with this exception. Defaults to <see cref="HttpStatusCode.InternalServerError"/>.</param>
	/// <param name="message">The message describing the error. Defaults to a generic error message.</param>
	/// <param name="innerException">The inner exception that caused the current exception. Optional.</param>
	public ApiException(HttpStatusCode? statusCode = null, string? message = null, Exception? innerException = null)
		: base(message, innerException)
	{
		CustomMessage = message;
		StatusCode = statusCode ?? HttpStatusCode.InternalServerError;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="ApiException"/> class with a specified message and optional inner exception, defaulting the status code to InternalServerError.
	/// </summary>
	/// <param name="message">The message describing the error.</param>
	/// <param name="innerException">The inner exception that caused the current exception. Optional.</param>
	public ApiException(string? message, Exception? innerException = null)
		: base(message, innerException)
	{
		CustomMessage = message;
		StatusCode = HttpStatusCode.InternalServerError;
	}
}