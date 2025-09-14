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
		
		// context.ActionArguments["pagination"] = _paginationParameters;
		// if (_paginationParameters == null)
		// {
		// 	_paginationParameters = new PaginationParameters();
		// 	context.ActionArguments["pagination"] = _paginationParameters;
		// }
		//
		// if (_paginationParameters.PageSize <= 0)
		// {
		// 	_paginationParameters.PageSize = PageSize;
		// }
	}

	public override void OnActionExecuted(ActionExecutedContext context)
	{
		if (_paginationParameters == null) return;

		if (context.Result is not ObjectResult { Value: IEnumerable<object> collection } objectResult) return;
		
		objectResult.Value = collection.ApplyPagination(_paginationParameters);
	}
}