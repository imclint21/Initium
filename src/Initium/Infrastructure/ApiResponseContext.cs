using System.Net;
using Microsoft.AspNetCore.Http;

namespace Initium.Infrastructure;

/// <summary>
/// Bridges plain <see cref="Results.ServiceResult"/> data objects to the current request's
/// <see cref="HttpContext"/>. When an action unwraps a result to its bare entity (losing the
/// result's <c>StatusCode</c> and <c>Metadata</c>), the unwrap stashes that response metadata here
/// so <see cref="Filters.ApiResponseMetadataFilter"/> can still write it to the HTTP response.
/// </summary>
public static class ApiResponseContext
{
	private const string StatusCodeKey = "__initium.statusCode";
	private const string MetadataKey = "__initium.metadata";

	private static IHttpContextAccessor? _accessor;

	/// <summary>Wired once from <c>AddInitium</c>.</summary>
	internal static void Use(IHttpContextAccessor accessor) => _accessor = accessor;

	/// <summary>
	/// Records the result's status code and metadata on the current request, if there is one.
	/// No-op outside an HTTP request (e.g. background workers), so unwrapping stays safe everywhere.
	/// </summary>
	internal static void Stash(HttpStatusCode? statusCode, IReadOnlyDictionary<string, string> metadata)
	{
		var httpContext = _accessor?.HttpContext;
		if (httpContext is null) return;

		if (statusCode is not null)
			httpContext.Items[StatusCodeKey] = statusCode.Value;

		if (metadata.Count > 0)
			httpContext.Items[MetadataKey] = new Dictionary<string, string>(metadata);
	}

	internal static HttpStatusCode? ReadStatusCode(HttpContext httpContext) =>
		httpContext.Items.TryGetValue(StatusCodeKey, out var value) ? (HttpStatusCode)value! : null;

	internal static IReadOnlyDictionary<string, string>? ReadMetadata(HttpContext httpContext) =>
		httpContext.Items.TryGetValue(MetadataKey, out var value) ? (IReadOnlyDictionary<string, string>)value! : null;
}
