using System.Reflection;
using Initium.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
public class QueryFilterAttribute : ActionFilterAttribute
{
	// Higher Order = inner filter = its OnActionExecuted runs first, i.e. before PaginateAttribute (Order 0) slices.
	private const int FilterOrder = 100;

	private readonly string _parameterName;
	private readonly string[] _propertySegments;
	private readonly ComparisonTypes _comparisonType;

	public QueryFilterAttribute(string parameterName, string propertyPath, ComparisonTypes comparisonType = ComparisonTypes.Equals)
	{
		_parameterName = parameterName;
		_propertySegments = propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		_comparisonType = comparisonType;
		Order = FilterOrder;
	}

	/// <inheritdoc cref="ActionFilterAttribute" />
	public override void OnActionExecuted(ActionExecutedContext context)
	{
		if (context.Result is not ObjectResult { Value: IEnumerable<object> collection } objectResult) return;

		if (!context.HttpContext.Request.Query.TryGetValue(_parameterName, out var parameterValues)) return;
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
