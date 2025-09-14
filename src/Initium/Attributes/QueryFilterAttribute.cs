using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using Initium.Constants;
using Initium.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Initium.Attributes;

// /// <summary>
// /// An attribute to filter query results based on a specified property and enum type.
// /// </summary>
// /// <typeparam name="T">The type of the objects being filtered.</typeparam>
// [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
// public class QueryFilterAttribute<T>(string propertyName, Type propertyType) : Attribute, IActionFilter
// {
// 	/// <summary>
// 	/// Called before the action method executes. No operation is performed in this implementation.
// 	/// </summary>
// 	/// <param name="context">The context for the executed action.</param>
// 	public void OnActionExecuting(ActionExecutingContext context) { }
// 	
// 	/// <summary>
// 	/// Executes after the action method is invoked. Filters the result based on the specified property and enum value.
// 	/// </summary>
// 	/// <param name="context">The context for the action filter.</param>
// 	public void OnActionExecuted(ActionExecutedContext context)
// 	{
// 		if (!propertyType.IsEnum)
// 			throw new ApiException(HttpStatusCode.BadRequest, $"QueryFilter only accepts enum types for filters. The property '{propertyName}' is not an enum.");
// 		
// 		if (context.Result is not ObjectResult { Value: IEnumerable<T> enumerable } objectResult) return;
// 		
// 		var query = context.HttpContext.Request.Query;
// 		if (!query.TryGetValue(propertyName, out var parameterValue)) return;
// 		
// 		if (StringValues.IsNullOrEmpty(parameterValue))
// 			throw new ApiException(HttpStatusCode.BadRequest, "Query parameter is required but was not provided.");
//
// 		if (!Enum.TryParse(propertyType, parameterValue, ignoreCase: true, out var enumValue))
// 			throw new ApiException(HttpStatusCode.BadRequest, $"Invalid query parameter value: `{parameterValue}` for `{propertyName}`. Allowed values are: {string.Join(", ", Enum.GetNames(propertyType))}.");
//
// 		objectResult.Value = enumerable.Where(item =>
// 		{
// 			var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
// 			if (property == null) return false;
// 			var value = property.GetValue(item);
// 			return value?.Equals(enumValue) == true;
// 		});
// 	}
// }

/// <summary>
/// A flexible query filter that applies filtering based on a property selector and comparison type.
/// </summary>
/// <typeparam name="T">The type of objects being filtered.</typeparam>
// [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
// public class QueryFilterAttribute<T>(Expression<Func<T, object>> propertySelector, ComparisonTypes comparisonType = ComparisonTypes.Equals) : Attribute, IActionFilter
// {
// 	public void OnActionExecuting(ActionExecutingContext context)
// 	{
// 	}
//
// 	public void OnActionExecuted(ActionExecutedContext context)
// 	{
// 		if (context.Result is not ObjectResult { Value: IEnumerable<T> enumerable } objectResult)
// 			return;
//
// 		var query = context.HttpContext.Request.Query;
// 		if (!query.TryGetValue(queryParameterName, out var parameterValue))
// 			return;
//
// 		var compiledSelector = propertySelector.Compile();
//
// 		objectResult.Value = enumerable.Where(item =>
// 		{
// 			var propertyValue = compiledSelector(item)?.ToString();
// 			if (propertyValue == null || string.IsNullOrEmpty(parameterValue))
// 				return false;
//
// 			return comparisonType switch
// 			{
// 				ComparisonTypes.StartsWith => propertyValue.StartsWith(parameterValue, StringComparison.OrdinalIgnoreCase),
// 				ComparisonTypes.Contains => propertyValue.Contains(parameterValue, StringComparison.OrdinalIgnoreCase),
// 				_ => propertyValue.Equals(parameterValue, StringComparison.OrdinalIgnoreCase)
// 			};
// 		}).ToList();
// 	}
// }


// return parameterValue != null && value != null && CompareValues(value, parameterValue);
//
// private bool CompareValues(object propertyValue, string parameterValue)
// {
// 	return !string.IsNullOrEmpty(parameterValue) && comparisonType switch
// 	{
// 		ComparisonType.StartsWith => propertyValue.ToString()?.StartsWith(parameterValue, StringComparison.OrdinalIgnoreCase) == true,
// 		ComparisonType.Contains => propertyValue.ToString()?.Contains(parameterValue, StringComparison.OrdinalIgnoreCase) == true,
// 		_ => propertyValue.ToString()?.Equals(parameterValue, StringComparison.OrdinalIgnoreCase) == true,
// 	};
// }