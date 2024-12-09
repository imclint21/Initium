using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Initium.Request;

public class PaginationParametersBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		ArgumentNullException.ThrowIfNull(bindingContext);

		var query = bindingContext.HttpContext.Request.Query;

		var pagination = new PaginationParameters
		{
			Page = int.TryParse(query["Page"], out var page) ? page : 1,
			PageSize = int.TryParse(query["PageSize"], out var pageSize) ? pageSize : 10
		};

		bindingContext.Result = ModelBindingResult.Success(pagination);
		return Task.CompletedTask;
	}
}