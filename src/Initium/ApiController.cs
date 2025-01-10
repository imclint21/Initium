using Microsoft.AspNetCore.Mvc;
using Initium.Filters;
using Initium.Infrastructure;

namespace Initium;

[ApiController]
[Route("[controller]")]
[TypeFilter(typeof(ApiExceptionFilter))]
[TypeFilter(typeof(ApiResponseFilter))]
[TypeFilter(typeof(LoggingFilter))]
[TypeFilter(typeof(ImplicitValidationFilter))]
public class ApiController : BaseController;

public class ApiController<TService>(TService service) : ApiController where TService : class
{
	public TService Service { get; } = service;
}
