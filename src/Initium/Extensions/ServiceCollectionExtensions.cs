using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Initium.Abstractions.Results;
using Initium.Abstractions.Transformers;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Initium.Extensions;

[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers and configures Initium framework services and conventions.
    /// Applies API conventions, route slugification, lowercase URL enforcement,
    /// JSON serialization customization, exception behavior for background services,
    /// and a standardized invalid model state response.
    /// This method is designed to augment existing MVC configurations without enforcing specific pipeline usage.
    /// </summary>
	public static IServiceCollection AddInitium(this IServiceCollection services)
	{
		// Lets ServiceResult.UnwrapOrThrow reach the current HttpContext so it can stash
		// the result's StatusCode + Metadata (e.g. Location) for the response filter to apply.
		services.AddHttpContextAccessor();

		services.Configure<MvcOptions>(options =>
			options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())));
		
		services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.Configure<HostOptions>(options => options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);
		
        services.Configure<JsonOptions>(options =>
        {
	        options.JsonSerializerOptions.WriteIndented = true;
	        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
		services.PostConfigure<ApiBehaviorOptions>(behaviorOptions => 
			behaviorOptions.InvalidModelStateResponseFactory = context => new InvalidModelStateResult(context));

		return services;
	}
}