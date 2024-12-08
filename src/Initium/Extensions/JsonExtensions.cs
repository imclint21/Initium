using System.Text.Json;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for serializing and deserializing JSON data.
/// </summary>
public static class JsonExtensions
{
	/// <summary>
	/// Serializes the specified object to a JSON string.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <returns>A JSON string representation of the object.</returns>
	public static string ToJson(this object obj) =>
		JsonSerializer.Serialize(obj);

	/// <summary>
	/// Serializes the specified object to a JSON string with indentation for readability.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <returns>A formatted JSON string representation of the object.</returns>
	public static string ToJsonF(this object obj) =>
		JsonSerializer.Serialize(obj, new JsonSerializerOptions
		{
			WriteIndented = true
		});

	/// <summary>
	/// Deserializes the specified JSON string to an object of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the object to deserialize to.</typeparam>
	/// <param name="value">The JSON string to deserialize.</param>
	/// <returns>An object of type <typeparamref name="T"/> if successful; otherwise, <c>null</c>.</returns>
	public static T? FromJson<T>(this string value) =>
		JsonSerializer.Deserialize<T>(value);

	/// <summary>
	/// Deserializes the specified JSON string to a <see cref="JsonElement"/> for dynamic handling of JSON data.
	/// </summary>
	/// <param name="value">The JSON string to deserialize.</param>
	/// <returns>A <see cref="JsonElement"/> representing the JSON data, or <c>null</c> if the deserialization fails.</returns>
	public static dynamic? FromJson(this string value) =>
		JsonSerializer.Deserialize<JsonElement>(value);
}