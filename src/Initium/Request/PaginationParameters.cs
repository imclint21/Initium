using Microsoft.AspNetCore.Mvc;
using Tapper;

namespace Initium.Request;

[TranspilationSource]
[ModelBinder(BinderType = typeof(PaginationParametersBinder))]
public class PaginationParameters
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 30;
}