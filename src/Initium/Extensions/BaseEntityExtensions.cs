using Initium.Entities;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for sequences of <see cref="BaseEntity"/>.
/// </summary>
public static class BaseEntityExtensions
{
	/// <summary>
	/// Orders a sequence of entities by ascending <see cref="BaseEntity.Weight"/>, then by
	/// <see cref="BaseEntity.CreatedAt"/> as a stable tie-breaker.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="source">The sequence to order.</param>
	/// <returns>The entities ordered by weight, then creation time.</returns>
	public static IOrderedEnumerable<T> OrderByWeight<T>(this IEnumerable<T> source) where T : BaseEntity =>
		source.OrderBy(entity => entity.Weight).ThenBy(entity => entity.CreatedAt);

	/// <summary>
	/// Orders a query of entities by ascending <see cref="BaseEntity.Weight"/>, then by
	/// <see cref="BaseEntity.CreatedAt"/> as a stable tie-breaker. The ordering is translated by the query
	/// provider (e.g. to <c>ORDER BY</c> in SQL), so prefer this over the <see cref="IEnumerable{T}"/> overload
	/// when the source is a database query.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="source">The query to order.</param>
	/// <returns>The entities ordered by weight, then creation time.</returns>
	public static IOrderedQueryable<T> OrderByWeight<T>(this IQueryable<T> source) where T : BaseEntity =>
		source.OrderBy(entity => entity.Weight).ThenBy(entity => entity.CreatedAt);

	/// <summary>
	/// Marks the entity as changed by setting <see cref="BaseEntity.UpdatedAt"/> to the current UTC time.
	/// Useful for bumping the timestamp without the EF save interceptor (e.g. an in-memory mutation).
	/// </summary>
	/// <param name="entity">The entity to touch.</param>
	public static void Touch(this BaseEntity entity) => entity.UpdatedAt = DateTimeOffset.UtcNow;
}
