using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Initium.Infrastructure;

[SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
public abstract class BaseService
{
    /// <summary>
    /// (Optional) Stores the current HTTP context associated with the service instance.
    /// </summary>
    public HttpContext? HttpContext { get; set; }
    
    /// <summary>
    /// (Optional) Holds the authenticated user represented as a ClaimsPrincipal.
    /// </summary>
    public ClaimsPrincipal? CurrentPrincipal { get; set; }
    
    /// <summary>
    /// (Optional) A dictionary for storing arbitrary service-specific metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
	
    /// <summary>
    /// Allows derived services or internal infrastructure components to assign properties on BaseService or its derivatives using a lambda expression.
    /// This method is designed for controlled internal assignment of service-specific context or metadata.
    /// </summary>
	protected internal TService Set<TService>(Action<TService> setter) where TService : BaseService
	{
		setter((TService)this);
		return (TService)this;
	}
}