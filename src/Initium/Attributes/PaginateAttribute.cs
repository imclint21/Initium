using Initium.Extensions;
using Initium.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Attributes;

/// <summary>
/// Automatically paginates <see cref="IEnumerable{T}"/> results and adds pagination headers to the response.
/// Use <c>PageSize = -1</c> to disable pagination while still returning the total count header.
/// </summary>
/// <remarks>
/// Override <see cref="ShouldPaginate"/> in a derived attribute to decide per request whether pagination applies
/// (for example to return the full list to certain callers). When it returns <see langword="false"/> the response
/// is left whole and only the total-count header is added.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class PaginateAttribute : ActionFilterAttribute
{
	private PaginationParameters? _paginationParameters;

	/// <summary>
	/// Gets or sets the default page size. Defaults to 30.
	/// </summary>
	public int PageSize { get; set; } = 30;

	/// <summary>
	/// Whether the current request should be paginated. Returns <see langword="true"/> by default; override to
	/// opt specific requests out of pagination (they then receive the whole collection and the total-count header).
	/// </summary>
	protected virtual bool ShouldPaginate(HttpContext context) => true;

	/// <inheritdoc />
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		// Prefer a bound PaginationParameters action argument; otherwise read page/pageSize straight from the query string
		// so the attribute works without the action having to declare a PaginationParameters parameter.
		_paginationParameters = context.ActionArguments.Values.FirstOrDefault(arg => arg is PaginationParameters) as PaginationParameters;

		if (_paginationParameters == null)
		{
			var query = context.HttpContext.Request.Query;
			_paginationParameters = new PaginationParameters
			{
				Page = int.TryParse(query["page"], out var page) ? page : 1,
				PageSize = int.TryParse(query["pageSize"], out var pageSize) ? pageSize : PageSize
			};
		}
		else
		{
			_paginationParameters.PageSize = PageSize;
		}
	}

	/// <inheritdoc />
	public override void OnActionExecuted(ActionExecutedContext context)
	{
		if (_paginationParameters == null) return;

		if (context.Result is not ObjectResult { Value: IEnumerable<object> collection } objectResult) return;

		var items = collection as IList<object> ?? collection.ToList();
		var totalCount = items.Count;

		if (_paginationParameters.PageSize >= 0 && ShouldPaginate(context.HttpContext))
		{
			var totalPages = (int)Math.Ceiling((double)totalCount / _paginationParameters.PageSize);

			context.HttpContext.Response.Headers["X-Pagination-Page"] = _paginationParameters.Page.ToString();
			context.HttpContext.Response.Headers["X-Pagination-PageSize"] = _paginationParameters.PageSize.ToString();
			context.HttpContext.Response.Headers["X-Pagination-TotalCount"] = totalCount.ToString();
			context.HttpContext.Response.Headers["X-Pagination-TotalPages"] = totalPages.ToString();

			objectResult.Value = items.ApplyPagination(_paginationParameters);
		}
		else
		{
			context.HttpContext.Response.Headers["X-Pagination-TotalCount"] = totalCount.ToString();
			objectResult.Value = items;
		}
	}
}
