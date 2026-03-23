using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for serving a static frontend (e.g., a SPA build) alongside the API.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class StaticFrontendExtensions
{
	/// <summary>
	/// Configures the application to serve static files and a default <c>index.html</c> from the specified directory.
	/// </summary>
	/// <param name="app">The application builder.</param>
	/// <param name="clientPath">The absolute path to the frontend build directory.</param>
	/// <returns>The application builder for chaining.</returns>
	public static IApplicationBuilder MapStaticFrontend(this IApplicationBuilder app, string clientPath)
	{
		app.UseDefaultFiles(new DefaultFilesOptions
		{
			FileProvider = new PhysicalFileProvider(clientPath),
			RequestPath = "",
			DefaultFileNames = new List<string> { "index.html" }
		});

		app.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new PhysicalFileProvider(clientPath),
			RequestPath = ""
		});

		return app;
	}
}