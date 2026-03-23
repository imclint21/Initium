using Initium.Extensions;
using Initium.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Initium.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PaginateAttribute : ActionFilterAttribute
{
	private PaginationParameters? _paginationParameters;
	public int PageSize { get; set; } = 30;

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		_paginationParameters = context.ActionArguments.Values.FirstOrDefault(arg => arg is PaginationParameters) as PaginationParameters;

		_paginationParameters ??= new PaginationParameters();
		_paginationParameters.PageSize = PageSize;
	}

	public override void OnActionExecuted(ActionExecutedContext context)
	{
		if (_paginationParameters == null) return;

		if (context.Result is not ObjectResult { Value: IEnumerable<object> collection } objectResult) return;

		var items = collection as IList<object> ?? collection.ToList();
		var totalCount = items.Count;

		if (_paginationParameters.PageSize >= 0)
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
