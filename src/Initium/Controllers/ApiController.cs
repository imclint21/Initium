using System.Net;
using Initium.Exceptions;
using Initium.Infrastructure;
using Initium.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Initium.Controllers;

/// <summary>
/// Base API controller providing global filters and routing.
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
/// Generic API controller that provides lazy access to a service of type TService.
/// </summary>
public abstract class ApiController<TService> : ApiController where TService : BaseService
{
	protected TService Service => (TService?)HttpContext.RequestServices.GetService(typeof(TService)) ?? throw new ApiException(HttpStatusCode.InternalServerError, $"Service of type `{typeof(TService).Name}` is not registered in the dependency injection container.");
}

// handle this
// public static ActionResult<T> OrFallbackWithStatusCode<T>(this T? baseEntity, HttpStatusCode statusCode) =>
// 	baseEntity != null ? new OkObjectResult(baseEntity) : new StatusCodeResult((int)statusCode);

// Add serializable to exceptions
// [Serializable]

// handle new/override
// return ServiceResult.Ok("COOL").As<SignUpResponse>();
// return ServiceResult<SignUpResponse>.Ok("COOL");