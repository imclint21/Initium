using System.Reflection;
using Initium.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace Initium.Attributes;

/// <summary>
/// Filters an <see cref="IEnumerable{T}"/> action result by a query-string parameter. The matched property is
/// resolved by name (dotted paths supported, e.g. <c>"PrepaidLine.Carrier.CountryCode"</c>) and compared using a
/// <see cref="ComparisonTypes"/> comparison. Stack multiple instances to filter on several fields.
/// </summary>
/// <remarks>
/// Attribute arguments must be compile-time constants, so the property is named as a string rather than a lambda.
/// The filter runs in <see cref="OnActionExecuted"/> with a high <see cref="IOrderedFilter.Order"/> so it executes
/// before <see cref="PaginateAttribute"/> (Order 0) — filtering happens before pagination slices the collection.
/// When the query parameter is absent or empty, the result is left untouched. Repeated query parameters are OR-ed.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class QueryFilterAttribute : ActionFilterAttribute, IActionModelConvention
{
	// Higher Order = inner filter = its OnActionExecuted runs first, i.e. before PaginateAttribute (Order 0) slices.
	private const int FilterOrder = 100;

	// A ParameterModel needs a ParameterInfo; the filter's query parameter is synthetic (no method argument backs it),
	// so we reuse the ParameterInfo of this string-typed stand-in to describe it as an optional string query value.
	private static readonly ParameterInfo StringParameter =
		typeof(QueryFilterAttribute).GetMethod(nameof(StringParameterTemplate), BindingFlags.NonPublic | BindingFlags.Static)!.GetParameters()[0];

	private readonly string[] _propertySegments;
	private readonly ComparisonTypes _comparisonType;

	/// <summary>The query-string parameter this filter reads.</summary>
	public string ParameterName { get; }

	/// <summary>Whether this filter is advertised in the API description (OpenAPI). Opt-in, so internal filters
	/// stay out of the public contract unless explicitly published.</summary>
	public bool Documented { get; }

	/// <summary>Human-readable summary of what the filter does, surfaced on the published parameter. Null when none.</summary>
	public string? Description { get; }

	public QueryFilterAttribute(
		string parameterName,
		string propertyPath,
		ComparisonTypes comparisonType = ComparisonTypes.Equals,
		bool documented = false,
		string? description = null)
	{
		ParameterName = parameterName;
		_propertySegments = propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		_comparisonType = comparisonType;
		Documented = documented;
		Description = description;
		Order = FilterOrder;
	}

	// Adds the filter's query parameter to the action's API description so it shows up in OpenAPI (optional string,
	// bound from the query string), the same way the attribute already reads it at runtime. Advertises the parameter
	// the endpoint actually honours without the developer repeating a [FromQuery] argument. Opt-in: only filters
	// declared documented are published, so internal filters stay out of the public contract. The Description is
	// carried on the attribute itself and applied to the published parameter by the consumer's OpenAPI transformer.
	void IActionModelConvention.Apply(ActionModel action)
	{
		if (!Documented)
			return;

		if (action.Parameters.Any(parameter => string.Equals(parameter.BindingInfo?.BinderModelName ?? parameter.ParameterName, ParameterName, StringComparison.OrdinalIgnoreCase)))
			return;

		action.Parameters.Add(new ParameterModel(StringParameter, [])
		{
			ParameterName = ParameterName,
			Action = action,
			BindingInfo = new BindingInfo { BindingSource = BindingSource.Query, BinderModelName = ParameterName },
		});
	}

	// Only exists so its parameter yields a ParameterInfo for an optional string; never called.
	private static void StringParameterTemplate(string? value) { }

	/// <inheritdoc cref="ActionFilterAttribute" />
	public override void OnActionExecuted(ActionExecutedContext context)
	{
		if (context.Result is not ObjectResult { Value: IEnumerable<object> collection } objectResult) return;

		if (!context.HttpContext.Request.Query.TryGetValue(ParameterName, out var parameterValues)) return;
		if (StringValues.IsNullOrEmpty(parameterValues)) return;

		var needles = parameterValues
			.Where(value => !string.IsNullOrWhiteSpace(value))
			.Select(value => value!.Trim())
			.ToArray();
		if (needles.Length == 0) return;

		objectResult.Value = collection.Where(item =>
		{
			var propertyValue = ResolveValue(item)?.ToString();
			if (string.IsNullOrEmpty(propertyValue)) return false;

			return needles.Any(needle => _comparisonType switch
			{
				ComparisonTypes.StartsWith => propertyValue.StartsWith(needle, StringComparison.OrdinalIgnoreCase),
				ComparisonTypes.Contains => propertyValue.Contains(needle, StringComparison.OrdinalIgnoreCase),
				_ => propertyValue.Equals(needle, StringComparison.OrdinalIgnoreCase)
			});
		}).ToList();
	}

	private object? ResolveValue(object? current)
	{
		foreach (var segment in _propertySegments)
		{
			if (current is null) return null;
			var property = current.GetType().GetProperty(segment, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
			if (property is null) return null;
			current = property.GetValue(current);
		}
		return current;
	}
}
