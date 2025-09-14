using Initium.Domain.Entities;

namespace Initium.Infrastructure.Extensions;

internal static class BaseEntityExtensions
{
	public static void Touch<TKey>(this BaseEntity<TKey> entity) => entity.UpdatedAt = DateTimeOffset.UtcNow;
}