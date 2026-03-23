using Microsoft.AspNetCore.Mvc;
using Tapper;

namespace Initium.Request;

/// <summary>
/// Represents pagination query parameters with page number and page size.
/// </summary>
[TranspilationSource]
[ModelBinder(BinderType = typeof(PaginationParametersBinder))]
public class PaginationParameters
{
	/// <summary>
	/// Gets or sets the current page number. Defaults to 1.
	/// </summary>
	public int Page { get; set; } = 1;

	/// <summary>
	/// Gets or sets the number of items per page. Defaults to 30. Use -1 to disable pagination.
	/// </summary>
	public int PageSize { get; set; } = 30;
}