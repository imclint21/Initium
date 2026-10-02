using Microsoft.AspNetCore.Http;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for reading request information from an <see cref="HttpContext"/>.
/// </summary>
public static class HttpContextExtensions
{
	/// <summary>
	/// Reads the public client IP from forwarding headers, for use behind a proxy (where
	/// <see cref="Microsoft.AspNetCore.Http.ConnectionInfo.RemoteIpAddress"/> is the proxy, not the client).
	/// Trust order: <c>X-Client-IP</c> (a custom header proxies do not rewrite), then the first hop of
	/// <c>X-Forwarded-For</c>, then <c>X-Real-IP</c>.
	/// </summary>
	/// <param name="httpContext">The current HTTP context.</param>
	/// <returns>The client IP address, or <c>null</c> if no forwarding header is present.</returns>
	public static string? ClientIpAddress(this HttpContext httpContext)
	{
		var headers = httpContext.Request.Headers;
		if (headers["X-Client-IP"].ToString() is { Length: > 0 } clientIp)
			return clientIp.Trim();
		if (headers["X-Forwarded-For"].ToString() is { Length: > 0 } forwardedFor)
			return forwardedFor.Split(',')[0].Trim();
		return headers["X-Real-IP"].ToString() is { Length: > 0 } realIp ? realIp : null;
	}

	/// <summary>
	/// Returns the request's <c>Origin</c> header without a trailing slash, or <c>null</c> when it is absent.
	/// Useful for building return URLs back to the calling front-end without storing its address on the API side.
	/// </summary>
	/// <param name="httpContext">The current HTTP context.</param>
	/// <returns>The origin without a trailing slash, or <c>null</c> if there is no <c>Origin</c> header.</returns>
	public static string? Origin(this HttpContext? httpContext) =>
		httpContext?.Request.Headers.Origin.ToString() is { Length: > 0 } origin
			? origin.TrimEnd('/')
			: null;

	/// <summary>
	/// Builds the public base URL of the API from the incoming request (<c>{scheme}://{host}</c>).
	/// </summary>
	/// <param name="request">The current HTTP request.</param>
	/// <returns>The scheme and host of the request, e.g. <c>https://api.acme.com</c>.</returns>
	public static string BaseUrl(this HttpRequest request) => $"{request.Scheme}://{request.Host}";
}
