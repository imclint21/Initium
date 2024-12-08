namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="Guid"/> objects.
/// </summary>
public static class GuidExtensions
{
	/// <summary>
	/// Converts the specified string to a <see cref="Guid"/>.
	/// </summary>
	/// <param name="value">The string representation of a GUID.</param>
	/// <returns>A <see cref="Guid"/> that is equivalent to the string.</returns>
	/// <exception cref="FormatException">Thrown if the string is not in a recognized GUID format.</exception>
	/// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
	public static Guid ToGuid(this string value) => Guid.Parse(value);
}