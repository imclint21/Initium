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
	
	
	/// <summary>
	/// Checks if an <see cref="IEnumerable{T}"/> is empty.
	/// </summary>
	/// <typeparam name="T">The type of elements in the enumerable.</typeparam>
	/// <param name="source">The enumerable to check.</param>
	/// <returns><c>true</c> if the enumerable is empty or null; otherwise, <c>false</c>.</returns>
	public static bool IsEmpty<T>(this IEnumerable<T>? source) => 
		source == null || !source.Any();
}
