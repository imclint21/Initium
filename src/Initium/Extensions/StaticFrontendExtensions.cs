using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Initium.Extensions;

[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class StaticFrontendExtensions
{
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

		// TODO: A quoi sert MapFallbackToFile?
		// app.MapFallbackToFile("index.html", Path.Combine(clientPath, "index.html"));

		return app;
	}
}