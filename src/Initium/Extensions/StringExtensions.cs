namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for working with strings.
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Determines whether the specified string is null or empty.
	/// </summary>
	/// <param name="value">The string to check.</param>
	/// <returns><c>true</c> if the string is null or empty; otherwise, <c>false</c>.</returns>
	public static bool IsNull(this string value) => string.IsNullOrEmpty(value);

	/// <summary>
	/// Determines whether the specified string is not null and not empty.
	/// </summary>
	/// <param name="value">The string to check.</param>
	/// <returns><c>true</c> if the string is not null and not empty; otherwise, <c>false</c>.</returns>
	public static bool IsNotNull(this string value) => !string.IsNullOrEmpty(value);

	/// <summary>
	/// Determines whether the specified string is empty after trimming whitespace.
	/// </summary>
	/// <param name="value">The string to check.</param>
	/// <returns><c>true</c> if the string is empty after trimming; otherwise, <c>false</c>.</returns>
	public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value.Trim());

	/// <summary>Trims the value, returning null when it is null, empty, or whitespace.</summary>
	public static string? TrimToNull(this string? value) =>
		string.IsNullOrWhiteSpace(value) ? null : value.Trim();

	/// <summary>Upper-cases the first character, leaving the rest untouched. Null/blank passes through unchanged.</summary>
	public static string? Capitalize(this string? value) =>
		string.IsNullOrWhiteSpace(value) ? value : char.ToUpperInvariant(value[0]) + value[1..];
}