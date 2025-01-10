using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Initium.Extensions;

/// <summary>
/// Provides extension methods for serializing objects to YAML format.
/// </summary>
public static class YamlExtensions
{
	/// <summary>
	/// Serializes the specified object to a YAML string with optional naming conventions.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="namingConvention">
	/// An optional naming convention for serialization. Defaults to <see cref="CamelCaseNamingConvention"/>.
	/// </param>
	/// <returns>A YAML string representation of the object.</returns>
	public static string ToYaml(this object obj, INamingConvention? namingConvention = null) =>
		new SerializerBuilder()
			.WithNamingConvention(namingConvention ?? CamelCaseNamingConvention.Instance)
			.Build()
			.Serialize(obj);
}