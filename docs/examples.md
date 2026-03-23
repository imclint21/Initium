# Examples

## Complete CRUD API

### Model

```csharp
public record Product(Guid Id, string Name, decimal Price);
```

### Request & Validator

```csharp
using Initium.Request;

public class CreateProductRequest : BaseRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

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

### Service

```csharp
using System.Net;
using Initium.Exceptions;
using Initium.Results;
using Initium.Services;

public class ProductService(ProductStore store) : BaseService
{
    public ServiceResult Create(CreateProductRequest request)
    {
        var product = new Product(Guid.NewGuid(), request.Name, request.Price);
        store.Add(product);
        return ServiceResult.Ok("Product created.", HttpStatusCode.Created);
    }

    public IEnumerable<Product> FindAll() =>
        store.ToList();

    public Product FindById(Guid id) =>
        store.FirstOrDefault(p => p.Id == id)
            ?? throw new ApiException(HttpStatusCode.NotFound, "Product not found.");

    public ServiceResult Delete(Guid id)
    {
        var product = store.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return ServiceResult.Error("Product not found.", HttpStatusCode.NotFound);

        store.Remove(product);
        return ServiceResult.Ok("Product deleted.");
    }
}
```

### Controller

```csharp
using Initium.Attributes;
using Initium.Controllers;
using Microsoft.AspNetCore.Mvc;

public class ProductsController : ApiController<ProductService>
{
    [HttpPost]
    [ValidateRequest(typeof(CreateProductRequestValidator))]
    [ApiResponse(201, "Product created successfully.")]
    [ApiResponse(400, "Validation error.")]
    public ActionResult Post([FromBody] CreateProductRequest request) =>
        Service.Create(request);

    [HttpGet]
    [Paginate(PageSize = 10)]
    public IEnumerable<Product> Get() =>
        Service.FindAll();

    [HttpGet("{id:guid}")]
    public Product Get(Guid id) =>
        Service.FindById(id);

    [HttpDelete("{id:guid}")]
    [ApiResponse(200, "Product deleted.")]
    [ApiResponse(404, "Product not found.")]
    public ActionResult Delete(Guid id) =>
        Service.Delete(id);
}
```

### Registration

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInitium();
builder.Services.AddControllers();
builder.Services.AddSingleton<ProductStore>();
builder.Services.AddTransient<ProductService>();

var app = builder.Build();
app.MapControllers();
app.Run();
```

---

## Result Chaining

A multi-step operation where each step can fail independently:

```csharp
public class OrderService : BaseService
{
    public ServiceResult PlaceOrder(OrderRequest request)
    {
        return ValidateStock(request.ProductId)
            .ChainWith(() => ReserveStock(request.ProductId, request.Quantity))
            .ChainWith(() => ChargePayment(request.PaymentInfo))
            .ChainWith(() => ServiceResult.Ok("Order placed.", HttpStatusCode.Created));
    }

    private ServiceResult ValidateStock(Guid productId)
    {
        // Check stock availability
        return ServiceResult.Ok();
    }

    private ServiceResult ReserveStock(Guid productId, int quantity)
    {
        // Reserve items
        return ServiceResult.Ok();
    }

    private ServiceResult ChargePayment(PaymentInfo info)
    {
        // Process payment
        return ServiceResult.Ok();
    }
}
```

---

## Typed Results with Unwrap

Returning and consuming typed data from services:

```csharp
public class AuthService : BaseService
{
    public async Task<ServiceResult<TokenResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return ServiceResult<TokenResponse>.Error("Invalid credentials.", HttpStatusCode.Unauthorized);

        var token = GenerateToken(user);
        return ServiceResult<TokenResponse>.Ok(new TokenResponse { Token = token });
    }
}
```

In the controller, use `UnwrapOrThrow` to extract data or throw:

```csharp
public class AuthController : ApiController<AuthService>
{
    [HttpPost("login")]
    [ApiResponse(200, "Login successful.")]
    [ApiResponse(401, "Invalid credentials.")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await Service.LoginAsync(request);
        return Ok(result.UnwrapOrThrow());
    }
}
```

---

## External HTTP Calls

`ServiceResult<TData>` converts from `HttpResponseMessage` automatically:

```csharp
public class WeatherService : BaseService
{
    public async Task<ServiceResult<WeatherData>> GetCurrentWeatherAsync(string city)
    {
        using var client = new HttpClient();
        return await client.GetAsync($"https://api.weather.com/{city}");
        // Automatically converts: success + JSON deserialization, or failure
    }
}
```

---

## Background Worker

A worker that runs periodic cleanup:

```csharp
public class ExpiredSessionCleanupWorker(SessionStore store)
    : BaseWorker(cycleDelay: TimeSpan.FromMinutes(10))
{
    protected override async Task DoWork(CancellationToken stoppingToken)
    {
        var expired = store.Where(s => s.ExpiresAt < DateTime.UtcNow).ToList();
        foreach (var session in expired)
            store.Remove(session);
    }
}
```

Register it:

```csharp
builder.Services.AddHostedService<ExpiredSessionCleanupWorker>();
```

---

## Binding Context to Services

Pass HTTP context, user identity, or custom values to services:

```csharp
public class ProfileController : ApiController<ProfileService>
{
    [HttpGet("me")]
    public ActionResult GetProfile() =>
        Service
            .Bind(User)           // ClaimsPrincipal
            .Bind(HttpContext)    // Full HTTP context
            .GetCurrentProfile();
}
```

Inside the service:

```csharp
public class ProfileService : BaseService
{
    public ServiceResult<UserProfile> GetCurrentProfile()
    {
        // UserId is automatically available from the bound ClaimsPrincipal
        if (UserId == null)
            return ServiceResult<UserProfile>.Error("Not authenticated.", HttpStatusCode.Unauthorized);

        var profile = _repository.FindByUserId(UserId.Value);
        return ServiceResult<UserProfile>.Ok(profile);
    }
}
```
