using Initium.Request;

namespace Initium.Extensions;

public static class PaginationExtensions
{
	public static IEnumerable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> source, PaginationParameters paginationParameters) => source
		.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
		.Take(paginationParameters.PageSize);
	
	public static IEnumerable<TModel> ApplyPagination<TModel>(this IEnumerable<TModel> source, PaginationParameters paginationParameters) => source
		.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
		.Take(paginationParameters.PageSize);
}