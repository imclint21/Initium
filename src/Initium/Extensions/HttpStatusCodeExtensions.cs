using System.Net;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for <see cref="HttpStatusCode"/>.
/// </summary>
public static class HttpStatusCodeExtensions
{
	/// <summary>
	/// Determines whether the status code indicates a successful response (2xx).
	/// </summary>
	/// <param name="statusCode">The HTTP status code to evaluate.</param>
	/// <returns><c>true</c> if the status code is between 200 and 299; otherwise, <c>false</c>.</returns>
	public static bool IsSuccess(this HttpStatusCode statusCode) =>
		statusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;
}