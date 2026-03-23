using System.Net;
using Initium.Exceptions;
using Initium.Infrastructure.Filters;
using Initium.Services;
using Microsoft.AspNetCore.Mvc;

namespace Initium.Controllers;

/// <summary>
/// Defines the base API controller within the Initium framework.
/// Applies essential filters such as exception handling, standardized API responses, and request logging.
/// Serves as the primary controller for HTTP API endpoints.
/// </summary>
[ApiController]
[Route("[controller]")]
[TypeFilter(typeof(ApiExceptionFilter))]
[TypeFilter(typeof(ApiResponseFilter))]
[TypeFilter(typeof(LoggingFilter))]
[TypeFilter(typeof(ImplicitValidationFilter))]
// [TypeFilter(typeof(CustomHeaderFilter))]
public abstract class ApiController : BaseController;

/// <summary>
/// Generic API controller that provides lazy access to a registered service of type <typeparamref name="TService"/>.
/// </summary>
/// <typeparam name="TService">The service type, must inherit from <see cref="BaseService"/>.</typeparam>
public abstract class ApiController<TService> : ApiController where TService : BaseService
{
	/// <summary>
	/// Gets the service instance resolved from the dependency injection container.
	/// </summary>
	protected TService Service => (TService?)HttpContext.RequestServices.GetService(typeof(TService)) ?? throw new ApiException(HttpStatusCode.InternalServerError, $"Service of type `{typeof(TService).Name}` is not registered in the dependency injection container.");
}
