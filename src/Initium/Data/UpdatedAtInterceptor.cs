using Initium.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Initium.Data;

/// <summary>
/// An EF Core <see cref="SaveChangesInterceptor"/> that stamps <see cref="BaseEntity.UpdatedAt"/> with the
/// current UTC time on every <see cref="BaseEntity"/> in the <see cref="EntityState.Modified"/> state, just
/// before changes are saved. Register it on the context with
/// <c>options.AddInterceptors(new UpdatedAtInterceptor())</c>.
/// </summary>
public class UpdatedAtInterceptor : SaveChangesInterceptor
{
	/// <inheritdoc />
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		Stamp(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	/// <inheritdoc />
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		Stamp(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void Stamp(DbContext? context)
	{
		if (context is null) return;
		foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
			if (entry.State == EntityState.Modified)
				entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
	}
}
