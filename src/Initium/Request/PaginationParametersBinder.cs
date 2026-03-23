using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Initium.Request;

/// <summary>
/// Custom model binder that extracts <see cref="PaginationParameters"/> from query string values.
/// </summary>
public class PaginationParametersBinder : IModelBinder
{
	/// <inheritdoc />
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		ArgumentNullException.ThrowIfNull(bindingContext);

		var query = bindingContext.HttpContext.Request.Query;

		var pagination = new PaginationParameters
		{
			Page = int.TryParse(query["Page"], out var page) ? page : 1,
			PageSize = int.TryParse(query["PageSize"], out var pageSize) ? pageSize : 30
		};

		bindingContext.Result = ModelBindingResult.Success(pagination);
		return Task.CompletedTask;
	}
}