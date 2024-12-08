using System.Net;

namespace Initium.Exceptions;

/// <summary>
/// Represents an exception specific to API behavior with additional context like HTTP status code and custom messages.
/// </summary>
public class ApiException : Exception
{
	/// <summary>
	/// Gets the HTTP status code associated with this exception.
	/// </summary>
	public HttpStatusCode StatusCode { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ApiException"/> class with a specified HTTP status code.
	/// </summary>
	/// <param name="statusCode">The HTTP status code associated with this exception.</param>
	public ApiException(HttpStatusCode statusCode)
	{
		StatusCode = statusCode;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="ApiException"/> class with a specified HTTP status code and message.
	/// </summary>
	/// <param name="statusCode">The HTTP status code associated with this exception.</param>
	/// <param name="message">The message describing the error.</param>
	public ApiException(HttpStatusCode statusCode, string message) : base(message)
	{
		StatusCode = statusCode;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ApiException"/> class with a specified HTTP status code, message, and inner exception.
	/// </summary>
	/// <param name="statusCode">The HTTP status code associated with this exception.</param>
	/// <param name="message">The message describing the error.</param>
	/// <param name="innerException">The inner exception that caused the current exception.</param>
	public ApiException(HttpStatusCode statusCode, string message, Exception? innerException) : base(message, innerException)
	{
		StatusCode = statusCode;
	}
}