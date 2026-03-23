# Controllers & Attributes

## ApiController

`ApiController` is the base class for all API controllers in Initium. It automatically applies:

- **ApiExceptionFilter** — catches `ApiException` and returns standardized error responses
- **ApiResponseFilter** — wraps all responses in a consistent `ApiResponse` envelope
- **LoggingFilter** — logs incoming requests
- **ImplicitValidationFilter** — validates model state automatically
- **Route slugification** — `ProductCategories` becomes `/product-categories`

```csharp
public class ProductsController : ApiController
{
    [HttpGet]
    public IEnumerable<Product> Get() => _repository.FindAll();
}
```

### Generic ApiController with Service Injection

Use `ApiController<TService>` to get lazy access to a registered service:

```csharp
public class ProductsController : ApiController<ProductService>
{
    [HttpPost]
    public ActionResult Create([FromBody] CreateProductRequest request) =>
        Service.Create(request);
}
```

The `Service` property resolves `TService` from the DI container automatically.

### Built-in Properties

Inherited from `BaseController`, all controllers have access to:

- `UserId` — the authenticated user's GUID (from `sub` or `NameIdentifier` claims)
- `ClientIp` — the client's IP address
- `UserAgent` — the client's User-Agent string
- `GetQueryParameter(key)` — retrieves a query string value

## Attributes

### `[ApiResponse]`

Documents the possible responses of an endpoint. Applied per-method or per-class:

```csharp
[HttpPost]
[ApiResponse(201, "Product created successfully.")]
[ApiResponse(400, "Invalid product data.")]
[ApiResponse(409, "A product with this name already exists.")]
public ActionResult Create([FromBody] CreateProductRequest request) =>
    Service.Create(request);
```

### `[ValidateRequest]`

Validates the request body using a [FluentValidation](https://docs.fluentvalidation.net/) validator before the action executes. Returns a `400 Bad Request` with structured errors on failure:

```csharp
[HttpPost]
[ValidateRequest(typeof(CreateProductRequestValidator))]
public ActionResult Create([FromBody] CreateProductRequest request) =>
    Service.Create(request);
```

The validator is a standard FluentValidation class:

```csharp
using FluentValidation;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

### `[Paginate]`

Automatically paginates `IEnumerable<T>` results. Accepts `page` and `pageSize` query parameters:

```csharp
[HttpGet]
[Paginate(PageSize = 20)]
public IEnumerable<Product> Get() => _repository.FindAll();
```

Request: `GET /products?page=2` returns items 21–40.

Default page size is 30 unless overridden in the attribute.

### `[CustomHeader]`

Adds custom HTTP headers to the response. Can be applied multiple times:

```csharp
[HttpGet("{id:guid}")]
[CustomHeader("X-Cache-Status", "MISS")]
[CustomHeader("X-Custom-Info", "Some value")]
public Product Get(Guid id) => _repository.FindById(id);
```

## ApiException

Throw an `ApiException` anywhere in your code to return a standardized error response:

```csharp
public Product FindById(Guid id)
{
    return _repository.FindById(id)
        ?? throw new ApiException(HttpStatusCode.NotFound, "Product not found.");
}
```

The `ApiExceptionFilter` on `ApiController` catches it and returns a proper `ApiResponse` with the specified status code and message.

## Standardized API Response

All responses from `ApiController` endpoints are wrapped in an `ApiResponse` envelope:

```json
{
  "message": "Product created successfully.",
  "statusCode": 201,
  "requestDetails": {
    "clientIp": "127.0.0.1",
    "endpoint": "/products",
    "userAgent": "Mozilla/5.0...",
    "correlationId": "0HN4..."
  },
  "data": { ... },
  "errors": null
}
```

## BaseService

Services should inherit from `BaseService` to gain access to:

- `HttpContext` — the current HTTP context (when bound)
- `CurrentPrincipal` — the authenticated `ClaimsPrincipal` (when bound)
- `UserId` — parsed from the principal's claims
- `Metadata` — arbitrary key-value store
- `Bindings` — typed dependency storage via `GetBinding<T>()`

### Binding Context to Services

Use the `Bind` extension methods to pass context from controllers to services:

```csharp
[HttpPost]
public ActionResult Create([FromBody] CreateProductRequest request) =>
    Service
        .Bind(HttpContext)
        .Bind(User)
        .Create(request);
```

## Background Workers

Use `BaseWorker` for recurring background tasks:

```csharp
public class CleanupWorker() : BaseWorker(cycleDelay: TimeSpan.FromMinutes(5))
{
    protected override async Task DoWork(CancellationToken stoppingToken)
    {
        // Cleanup logic here
    }
}
```

Features:
- Configurable cycle delay (default: 30 seconds)
- Optional `LaunchConditions` to control eligibility
- `Restart()` method to reset the worker cycle
- Automatic error resilience — exceptions are caught and the worker continues

## Static Frontend Serving

Serve a static frontend (e.g., a SPA build) alongside your API:

```csharp
app.MapStaticFrontend(Path.Combine(app.Environment.WebRootPath, "Client"));
```

This configures both default files (`index.html`) and static file serving from the specified directory.
