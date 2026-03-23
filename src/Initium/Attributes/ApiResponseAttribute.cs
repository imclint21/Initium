using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Initium.Attributes;

/// <summary>
/// Documents a possible API response for an endpoint, specifying a status code and description message.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ApiResponseAttribute(HttpStatusCode statusCode, string message) : Attribute
{
	/// <summary>
	/// Gets the HTTP status code for this response.
	/// </summary>
	public HttpStatusCode StatusCode { get; } = statusCode;

	/// <summary>
	/// Gets the description message for this response.
	/// </summary>
	public string Message { get; } = message;

	/// <summary>
	/// Initializes a new instance of <see cref="ApiResponseAttribute"/> with an integer status code.
	/// </summary>
	/// <param name="statusCode">The HTTP status code as an integer.</param>
	/// <param name="message">The description message.</param>
	public ApiResponseAttribute(int statusCode, string message) : this((HttpStatusCode)statusCode, message)
	{
	}
}
