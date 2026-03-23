# Getting Started

## Installation

Add the Initium NuGet package to your project:

```bash
dotnet add package Initium
```

## Setup

Register Initium services in your `Program.cs`. This configures route slugification, lowercase URLs, JSON serialization with enum support, and standardized model validation responses.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInitium();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();
```

## Your First Controller

Create a controller by inheriting from `ApiController`:

```csharp
using Initium.Controllers;
using Initium.Attributes;
using Microsoft.AspNetCore.Mvc;

public class CoffeeController(CoffeeService service) : ApiController
{
    [HttpPost]
    [ApiResponse(200, "Coffee prepared successfully.")]
    [ApiResponse(400, "An error occurred during the preparation process.")]
    public ActionResult Prepare() => service.PrepareCoffee();
}
```

## Your First Service

Create a service by inheriting from `BaseService`. Each method returns a `ServiceResult`:

```csharp
using System.Net;
using Initium.Results;
using Initium.Services;

public class CoffeeService : BaseService
{
    public ServiceResult PrepareCoffee()
    {
        var grindResult = GrindBeans();
        if (grindResult.Failed) return grindResult;

        return ServiceResult.Ok("The coffee is ready!");
    }

    private ServiceResult GrindBeans()
    {
        return ServiceResult.Ok();
    }
}
```

Don't forget to register your service in the DI container:

```csharp
builder.Services.AddTransient<CoffeeService>();
```

## What's Next?

- Learn about [Service Results](service-results.md) for advanced result handling and chaining
- Explore [Controllers & Attributes](controllers.md) for validation, pagination, and custom headers
- See [Examples](examples.md) for full working scenarios
