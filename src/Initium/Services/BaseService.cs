using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Initium.Services;

/// <summary>
/// Provides a foundational service class within the Initium framework.
/// Encapsulates common properties such as the current HTTP context, authenticated principal, and service-specific metadata.
/// Designed to be inherited by specific services to simplify dependency management and context propagation.
/// </summary>
[SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
public abstract class BaseService
{
	public Dictionary<Type, object> Bindings { get; set; } = new();
	
    /// <summary>
    /// (Optional) Stores the current HTTP context associated with the service instance.
    /// </summary>
    public HttpContext? HttpContext { get; set; }
    
    /// <summary>
    /// (Optional) Holds the authenticated user represented as a ClaimsPrincipal.
    /// </summary>
    public ClaimsPrincipal? CurrentPrincipal { get; set; }

    protected Guid? UserId => Guid.TryParse(CurrentPrincipal?.FindFirst("sub")?.Value ?? CurrentPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var guid) ? guid : null;

    /// <summary>
    /// (Optional) A dictionary for storing arbitrary service-specific metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
	
    /// <summary>
    /// Allows derived services or internal infrastructure components to assign properties on BaseService or its derivatives using a lambda expression.
    /// This method is designed for controlled internal assignment of service-specific context or metadata.
    /// </summary>
    public TService Set<TService>(Action<TService> setter) where TService : BaseService
	{
		setter((TService)this);
		return (TService)this;
	}

	protected TValue? GetBinding<TValue>() => Bindings.TryGetValue(typeof(TValue), out var value) ? (TValue)value : default;
}

// public class BaseService<TEntity> : BaseService where TEntity : BaseEntity;
// public class BaseService<TEntity, TKey> : BaseService where TEntity : BaseEntity<TKey>;