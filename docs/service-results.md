# Service Results

`ServiceResult` is the core abstraction in Initium for representing the outcome of service operations. It carries success/failure state, an optional message, an HTTP status code, and structured errors.

## Basic Usage

### Success

```csharp
// Simple success (implicit 200)
return ServiceResult.Ok();

// Success with a message
return ServiceResult.Ok("Order placed successfully.");

// Success with a specific status code
return ServiceResult.Ok(HttpStatusCode.Created);

// Success with message and status code
return ServiceResult.Ok("Item created.", HttpStatusCode.Created);
```

### Error

```csharp
// Generic error (defaults to 500)
return ServiceResult.Error();

// Error with a message
return ServiceResult.Error("Something went wrong.");

// Error with a specific status code
return ServiceResult.Error(HttpStatusCode.Conflict);

// Error with message and status code
return ServiceResult.Error("Email already exists.", HttpStatusCode.Conflict);

// Error from an exception
return ServiceResult.Error(exception);
```

### Structured Errors

You can attach detailed `ApiError` objects for richer error responses:

```csharp
var errors = new[]
{
    new ApiError("INVALID_EMAIL", "The email format is invalid."),
    new ApiError("WEAK_PASSWORD", "Password must be at least 8 characters.")
};

return ServiceResult.Error("Validation failed.", errors, HttpStatusCode.BadRequest);
```

## Typed Results with `ServiceResult<TData>`

When your operation returns data, use the generic variant:

```csharp
public ServiceResult<Product> GetProduct(Guid id)
{
    var product = _repository.FindById(id);
    if (product == null)
        return ServiceResult<Product>.Error("Product not found.", HttpStatusCode.NotFound);

    return ServiceResult<Product>.Ok(product);
}
```

### Implicit Conversions

`ServiceResult<TData>` supports implicit conversion from data, so you can return data directly:

```csharp
public ServiceResult<Product> GetProduct(Guid id)
{
    var product = _repository.FindById(id);
    if (product == null)
        return ServiceResult<Product>.Error("Product not found.", HttpStatusCode.NotFound);

    // Implicitly wraps in a successful ServiceResult<Product>
    return product;
}
```

It also converts implicitly from `HttpResponseMessage`, deserializing the JSON body:

```csharp
public async Task<ServiceResult<WeatherData>> GetWeatherAsync()
{
    using var client = new HttpClient();
    return await client.GetAsync("https://api.weather.com/data");
}
```

## Chaining Results

### Manual Chaining

Check the result of each step before proceeding:

```csharp
public ServiceResult PlaceOrder(OrderRequest request)
{
    var stockResult = CheckStock(request.ProductId);
    if (stockResult.Failed) return stockResult;

    var paymentResult = ProcessPayment(request.Amount);
    if (paymentResult.Failed) return paymentResult;

    return ServiceResult.Ok("Order placed.");
}
```

### Using `ChainWith`

Chain operations fluently — if any step fails, the chain stops and returns that failure:

```csharp
public ServiceResult PlaceOrder(OrderRequest request)
{
    return CheckStock(request.ProductId)
        .ChainWith(() => ProcessPayment(request.Amount))
        .ChainWith(() => ServiceResult.Ok("Order placed."));
}
```

## Boolean Conversion

`ServiceResult` implicitly converts to `bool`, making conditional checks natural:

```csharp
var result = DoSomething();
if (!result) return result;  // equivalent to: if (result.Failed)
```

## Fluent Modifiers

Modify a result after creation:

```csharp
// Set message based on outcome
return result.WithMessage(success => success ? "Done!" : "Failed.");

// Override status code
return result.WithStatusCode(HttpStatusCode.Accepted);

// Dynamic status code
return result.WithStatusCode(success => success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);

// Attach metadata (mapped to HTTP response headers)
return result.WithMetadata("X-Request-Id", requestId);
```

## Unwrapping Data

For `ServiceResult<TData>`, extract the data safely:

```csharp
// Returns data or null
var data = result.Unwrap();

// Returns data or throws ApiException if null
var data = result.UnwrapOrThrow();

// With custom error details
var data = result.UnwrapOrThrow(HttpStatusCode.NotFound, "Resource not found.");
```

## Converting Between Result Types

Use extension methods to convert results:

```csharp
// Attach data to a plain ServiceResult
ServiceResult<Product> typedResult = baseResult.WithData(product);

// Convert to a typed result without data (preserves status/message)
ServiceResult<Product> typedResult = baseResult.As<Product>();
```

## Identity Integration

`ServiceResult` integrates with ASP.NET Core Identity. `IdentityResult` converts implicitly:

```csharp
public async Task<ServiceResult> RegisterUser(RegisterRequest request)
{
    var user = new ApplicationUser { Email = request.Email };
    return await _userManager.CreateAsync(user, request.Password);
    // IdentityResult is automatically converted to ServiceResult
}
```
