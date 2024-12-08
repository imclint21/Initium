namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="IEnumerable{T}"/> of strings.
/// </summary>
public static class EnumerableExtensions
{
	/// <summary>
	/// Concatenates the elements of a sequence of strings, using the specified separator between each element.
	/// </summary>
	/// <param name="value">The sequence of strings to join.</param>
	/// <param name="separator">The string to use as a separator. The default is a single space (" ").</param>
	/// <returns>A single string that consists of the elements in the sequence, delimited by the separator.</returns>
	public static string Join(this IEnumerable<string> value, string separator = " ") =>
		string.Join(separator, value);
}