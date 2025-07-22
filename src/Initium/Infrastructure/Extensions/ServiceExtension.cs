using System.Security.Claims;
using Initium.Services;
using Microsoft.AspNetCore.Http;

namespace Initium.Infrastructure.Extensions;

public static class ServiceExtensions
{
	public static TService Bind<TService>(this TService service, ClaimsPrincipal user) where TService : BaseService => 
		service.Set<TService>(svc => svc.CurrentPrincipal = user);

	public static TService Bind<TService>(this TService service, HttpContext httpContext) where TService : BaseService => 
	    service.Set<TService>(svc => svc.HttpContext = httpContext);
	
	public static TService Bind<TService>(this TService service, string key, object value) where TService : BaseService => 
		service.Set<TService>(svc => svc.Metadata[key] = value);
    
	public static TService Bind<TService, TValue>(this TService service, TValue value) where TService : BaseService => 
		service.Set<TService>(svc => svc.Bindings[typeof(TValue)] = value ?? throw new ArgumentNullException(nameof(value)));
}
