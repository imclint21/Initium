using Initium.Request;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for applying pagination to collections and queryables.
/// </summary>
public static class PaginationExtensions
{
	/// <summary>
	/// Applies pagination to an <see cref="IQueryable{T}"/>. Returns all items if <see cref="PaginationParameters.PageSize"/> is negative.
	/// </summary>
	public static IEnumerable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> source, PaginationParameters paginationParameters)
	{
		if (paginationParameters.PageSize < 0) return source;

		return source
			.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
			.Take(paginationParameters.PageSize);
	}

	/// <summary>
	/// Applies pagination to an <see cref="IEnumerable{T}"/>. Returns all items if <see cref="PaginationParameters.PageSize"/> is negative.
	/// </summary>
	public static IEnumerable<TModel> ApplyPagination<TModel>(this IEnumerable<TModel> source, PaginationParameters paginationParameters)
	{
		if (paginationParameters.PageSize < 0) return source;

		return source
			.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
			.Take(paginationParameters.PageSize);
	}
}
