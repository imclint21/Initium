<p align="center">
  <img src="https://raw.githubusercontent.com/imclint21/Initium/master/src/Initium/logo.png" height="100" alt="Initium" />
</p>

<h3 align="center">Initium</h3>

<p align="center">
  Service Core Foundation for .NET API development.
</p>

<p align="center">
  <a href="https://github.com/imclint21/Initium/actions/workflows/publish.yml"><img src="https://github.com/imclint21/Initium/actions/workflows/publish.yml/badge.svg?branch=master" /></a>
  <a href="https://www.nuget.org/packages/Initium/"><img src="https://img.shields.io/nuget/v/Initium" /></a>
  <img src="https://img.shields.io/badge/.NET-8.0%20LTS-blue" />
  <img src="https://img.shields.io/badge/license-MIT-lightgrey.svg" />
</p>

## Introduction

Initium is a library for simplifying .NET API development, offering standardized service operations, flexible routing, and seamless chaining of service results for cleaner and more maintainable code.

- **Streamlined API Controllers**: Simplifies response handling with attributes like `[ApiResponse]`, providing clear and consistent HTTP status documentation.
- **Centralized Result Management**: `ServiceResult` enables clear success or failure status, making error handling and conditional logic seamless.
- **Exception Handling Made Easy**: `ApiException` provides a straightforward way to handle specific HTTP error codes, improving code clarity.
- **Result Chaining**: Methods return `ServiceResult` or typed results, allowing for intuitive chaining and cleaner service logic.
- **Enhanced Maintainability**: Standardized patterns reduce boilerplate code, making APIs easier to build, understand, and maintain.

## Getting Started

To get started with Initium, just add the package using NuGet:

```bash
dotnet add package Initium
```

## How does it Work?

Here's how to create a controller :

```csharp
public class CoffeeController(CoffeeService service) : ApiController
{
    [HttpPost]
    [ApiResponse(200, "Coffee prepared successfully.")]
    [ApiResponse(400, "An error occurred during the preparation process.")]
    public ActionResult PrepareCoffee() => service.PrepareCoffee();
}
```
And here's how to create an action in a service, each function returns a `ServiceResult`, and can be chained.

```csharp
public class CoffeeService
{
    public ServiceResult DoSomething()
    {
        return ServiceResult.Error("Something happened!", HttpStatusCode.Conflict);
    }

    public ServiceResult PrepareCoffee()
    {
        var doSomethingResult = DoSomething();
        if (doSomethingResult == false) return doSomethingResult;

        return ServiceResult.Ok("The coffee is now DONE!");
    }
}
```

## Documentation

See the [docs/](docs/) folder for detailed documentation:

- [Getting Started](docs/getting-started.md)
- [Service Results](docs/service-results.md)
- [Controllers & Attributes](docs/controllers.md)
- [Examples](docs/examples.md)

## Contribute to Initium

See [CONTRIBUTING.md](CONTRIBUTING.md) for best practices and instructions on setting up your development environment to work on Initium.
