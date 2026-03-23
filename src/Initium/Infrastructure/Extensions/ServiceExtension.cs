using System.Security.Claims;
using Initium.Services;
using Microsoft.AspNetCore.Http;

namespace Initium.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for binding context data to <see cref="BaseService"/> instances.
/// </summary>
public static class ServiceExtensions
{
	/// <summary>
	/// Binds the authenticated user's <see cref="ClaimsPrincipal"/> to the service.
	/// </summary>
	public static TService Bind<TService>(this TService service, ClaimsPrincipal user) where TService : BaseService =>
		service.Set<TService>(svc => svc.CurrentPrincipal = user);

	/// <summary>
	/// Binds the current <see cref="HttpContext"/> to the service.
	/// </summary>
	public static TService Bind<TService>(this TService service, HttpContext httpContext) where TService : BaseService =>
	    service.Set<TService>(svc => svc.HttpContext = httpContext);

	/// <summary>
	/// Binds an arbitrary metadata key-value pair to the service.
	/// </summary>
	public static TService Bind<TService>(this TService service, string key, object value) where TService : BaseService =>
		service.Set<TService>(svc => svc.Metadata[key] = value);

	/// <summary>
	/// Binds a typed value to the service, retrievable via <c>GetBinding&lt;TValue&gt;()</c>.
	/// </summary>
	public static TService Bind<TService, TValue>(this TService service, TValue value) where TService : BaseService =>
		service.Set<TService>(svc => svc.Bindings[typeof(TValue)] = value ?? throw new ArgumentNullException(nameof(value)));
}
