using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Initium.Controllers;

/// <summary>
/// Serves as the foundational controller class in the Initium framework.
/// Provides access to common HTTP request data such as UserId, Client IP, and User-Agent,
/// and includes utility methods for derived API controllers.
/// Intended for general-purpose controller logic across the application.
/// </summary>
public abstract class BaseController : ControllerBase
{
	/// <summary>
	/// Gets the GUID of the currently authenticated user, if available.
	/// Extracted from the "sub" or "NameIdentifier" claims.
	/// </summary>
	protected Guid? UserId => Guid.TryParse(User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var guid) ? guid : null;

	/// <summary>
	/// Gets the IP address of the client making the current request.
	/// </summary>
	protected string? ClientIp => HttpContext.Connection.RemoteIpAddress?.ToString();

	/// <summary>
	/// Gets the User-Agent string from the current HTTP request headers.
	/// </summary>
	protected string? UserAgent => HttpContext.Request.Headers.UserAgent;
	
	/// <summary>
	/// Retrieves the specified query string parameter value from the current HTTP request.
	/// Returns null if the parameter does not exist.
	/// </summary>
	/// <param name="key">The query parameter key.</param>
	/// <returns>The parameter value if found; otherwise, null.</returns>
	protected string? GetQueryParameter(string key) =>
		HttpContext.Request.Query.TryGetValue(key, out var value) ? value.FirstOrDefault() : null;
}

// public class BaseController<TService>(TService service) : ApiController where TService : BaseService
// {
// 	protected readonly TService Service = service;
// }