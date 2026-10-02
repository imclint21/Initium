using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Tapper;

namespace Initium.Entities;

/// <summary>
/// Base for entities: a key and creation/update timestamps, plus free-form bookkeeping fields.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
[TranspilationSource]
public abstract class BaseEntity<TKey>
{
	// Identity and timestamps lead the JSON; the free-form bookkeeping fields (weight/metadata/tags) sink
	// below the derived entity's own properties (which sit at the default order of 0).
	[Key]
	[JsonPropertyOrder(-3)]
	public TKey Id { get; set; } = default!;

	[JsonPropertyOrder(-2)]
	public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

	[JsonPropertyOrder(-1)]
	public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

	/// <summary>Manual ordering weight — lower comes first.</summary>
	[JsonPropertyOrder(1)]
	public int Weight { get; set; }

	/// <summary>Free-form metadata — a bag of key/value pairs whose shape is up to the caller. Values may be any
	/// JSON scalar (string, number, boolean), not just strings. Stored as jsonb. Null when none.</summary>
	[Column(TypeName = "jsonb")]
	[JsonPropertyOrder(2)]
	public Dictionary<string, object>? Metadata { get; set; }

	/// <summary>Free-form tags for grouping/filtering. Null when none.</summary>
	[JsonPropertyOrder(3)]
	public string[]? Tags { get; set; }
}

/// <summary>Base for entities keyed by a <see cref="Guid"/>, auto-assigned on creation.</summary>
[TranspilationSource]
public abstract class BaseEntity : BaseEntity<Guid>
{
	protected BaseEntity() => Id = Guid.NewGuid();
}
