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
	// protected string? GetClaim(string claimType) => HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
	// protected string UserId => HttpContext.User.Claims.First(claim => claim.Type == "sub").Value;
	// protected Guid UserId => Guid.TryParse(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value, out var authorizationClaimUid) ? authorizationClaimUid : default;
}

// public class BaseController<TService>(TService service) : ApiController where TService : BaseService
// {
// 	protected readonly TService Service = service;
// }