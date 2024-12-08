namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for working with objects.
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// Determines whether the specified object is null.
	/// </summary>
	/// <param name="value">The object to check.</param>
	/// <returns><c>true</c> if the object is null; otherwise, <c>false</c>.</returns>
	public static bool IsNull(this object? value) => value == null;

	/// <summary>
	/// Determines whether the specified object is not null.
	/// </summary>
	/// <param name="value">The object to check.</param>
	/// <returns><c>true</c> if the object is not null; otherwise, <c>false</c>.</returns>
	public static bool IsNotNull(this object? value) => value != null;

	/// <summary>
	/// Determines whether the specified object is equal to the default value for its type.
	/// </summary>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <param name="value">The object to check.</param>
	/// <returns><c>true</c> if the object is equal to the default value for its type; otherwise, <c>false</c>.</returns>
	public static bool IsDefault<T>(this object value) => Equals(value, default(T));
}